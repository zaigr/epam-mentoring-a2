using System.IO;
using System.Linq;
using System.Text;
using Client.MessageQueue.Builders;
using Client.MessageQueue.Messages;
using Xunit;

namespace Client.MessageQueue.Tests.Unit.Builders
{
    public class MessageSequenceBuilderTests
    {
        private readonly IMessageSequenceBuilder _builder = new MessageSequenceBuilder();

        [Fact]
        public async void GivenStream_WhenStreamEmpty_ThenNoMessagesCreated()
        {
            // Arrange
            var emptyStream = new MemoryStream();

            // Act
            var messages = await _builder.CreateSequenceAsync<FileContentMessage>(emptyStream, messageSizeBytes: 20);

            // Assert
            Assert.Empty(messages);
        }

        [Fact]
        public async void GivenStream_WhenContentPresented_ThenContentMessagesCreated()
        {
            // Arrange
            var content = new string('A', count: 1266);
            var inputBuffer = Encoding.UTF8.GetBytes(content);

            var stream = new MemoryStream(inputBuffer);

            // Act
            var messageSizeBytes = 50;
            var messages = await _builder.CreateSequenceAsync<FileContentMessage>(stream, messageSizeBytes);

            // Assert
            Assert.NotEmpty(messages);

            var expectedCount = (inputBuffer.Length / messageSizeBytes) + 1;
            Assert.Equal(expectedCount, messages.Count());

            var sessionId = messages.First().SessionId;
            Assert.NotNull(sessionId);
            Assert.All(messages, m => Assert.Equal(sessionId, m.SessionId));
        }
    }
}
