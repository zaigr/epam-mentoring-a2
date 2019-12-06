using System;
using System.IO;
using System.Threading.Tasks;
using Client.Core.Monitoring.EventArgs;
using Client.MessageQueue;
using Client.MessageQueue.Builders;
using Client.MessageQueue.Messages;
using Serilog;

namespace Client.Core.Handling
{
    public class ResourceStreamingHandler : IResourceMonitorEventHandler
    {
        private readonly IMessageSequenceBuilder _messageSequenceBuilder;

        private readonly IMessageSender _messageSender;

        private readonly Func<int> _messageSizeBytes;

        public ResourceStreamingHandler(
            IMessageSequenceBuilder messageSequenceBuilder,
            IMessageSender messageSender,
            Func<int> messageSizeBytes)
        {
            _messageSequenceBuilder = messageSequenceBuilder;
            _messageSender = messageSender;
            _messageSizeBytes = messageSizeBytes;
        }

        public async void ResourceAddedEventHandler(object sender, ResourceAddedEventArgs eventArgs)
        {
            try
            {
                Log.Information($"Resource added handler invoked, resource: '{eventArgs.ResourceName}'");

                await StreamFileToMessageQueueAsync(eventArgs.ResourcePath, eventArgs.ResourceName);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception raised during resource added event processing.");
            }
        }

        public async void ResourceChangedEventHandler(object sender, ResourceChangedEventArgs eventArgs)
        {
            try
            {
                Log.Information($"Resource changed handler invoked, resource: '{eventArgs.ResourceName}'");

                await StreamFileToMessageQueueAsync(eventArgs.ResourcePath, eventArgs.ResourceName);
            }
            catch (Exception e)
            {
                Log.Error(e, "Exception raised during resource added event processing.");
            }
        }

        private async Task StreamFileToMessageQueueAsync(string filePath, string fileName)
        {
            var fullPath = Path.Combine(filePath, fileName);
            Log.Debug($"Read file from {filePath}");

            using (var fileStream = File.OpenRead(fullPath))
            {
                var messages = await _messageSequenceBuilder.CreateSequenceAsync<FileContentMessage>(fileStream, _messageSizeBytes());

                foreach (var message in messages)
                {
                    message.FileName = fileName;

                    await _messageSender.SendAsync(message);
                }
            }
        }
    }
}
