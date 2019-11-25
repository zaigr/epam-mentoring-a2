using System;
using System.Threading.Tasks;
using Client.MessageQueue;
using Client.MessageQueue.Messages;
using Serilog;

namespace Client.ScanService.Configuration.External
{
    public class ExternalConfigurationProvider : IExternalConfigurationProvider
    {
        private readonly IMessageReceiver _messageReceiver;

        public ExternalConfigurationProvider(IMessageReceiver messageReceiver)
        {
            _messageReceiver = messageReceiver;
        }

        public void SyncExternalConfiguration(ServiceConfig config)
        {
            _messageReceiver.RegisterHandler<ClientConfigurationMessage>(
                message =>
                {
                    HandleMessage(message, config);
                    return Task.CompletedTask;
                },
                HandleException);
        }

        private void HandleMessage(ClientConfigurationMessage message, ServiceConfig config)
        {
            Log.Information(
                $"Message size updated from {config.MessageMaxSizeBytes} " +
                $"to {message.MaxClientMessageSizeBytes}");

            config.MessageMaxSizeBytes = message.MaxClientMessageSizeBytes;

            ServiceConfigManager.UpdateConfigSource(
                c => c.MessageMaxSizeBytes,
                config.MessageMaxSizeBytes);
        }

        private void HandleException(Exception exception)
        {
            Log.Error(exception, "Exception raised during topic message handling.");
        }
    }
}
