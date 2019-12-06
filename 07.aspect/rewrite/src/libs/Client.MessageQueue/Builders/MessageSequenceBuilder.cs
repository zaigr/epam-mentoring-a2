using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Client.MessageQueue.Messages.Base;

namespace Client.MessageQueue.Builders
{
    public class MessageSequenceBuilder : IMessageSequenceBuilder
    {
        public async Task<IEnumerable<TMessage>> CreateSequenceAsync<TMessage>(Stream contentStream, int messageSizeBytes)
            where TMessage : ContentMessageBase, new()
        {
            var sessionId = Guid.NewGuid().ToString();
            var messages = new List<TMessage>();

            var buffer = ArrayPool<byte>.Shared.Rent(messageSizeBytes);
            while (await contentStream.ReadAsync(buffer, 0, messageSizeBytes) > 0)
            {
                var message = new TMessage
                {
                    Content = buffer.ToArray(),
                    SessionId = sessionId,
                };

                messages.Add(message);
            }

            ArrayPool<byte>.Shared.Return(buffer);

            return messages;
        }
    }
}
