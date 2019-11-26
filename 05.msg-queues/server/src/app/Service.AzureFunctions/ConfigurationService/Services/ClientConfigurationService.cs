using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Service.MessageQueue;
using Service.MessageQueue.Messages;

namespace Service.AzureFunctions.ConfigurationService.Services
{
    public class ClientConfigurationService : IClientConfigurationService
    {
        private readonly IMessageSender _messageSender;

        private readonly ILogger _log;

        private readonly int _maxClientMessageSizeBytes;

        public ClientConfigurationService(
            IMessageSender messageSender,
            ILogger log,
            int maxClientMessageSizeBytes)
        {
            _messageSender = messageSender;
            _maxClientMessageSizeBytes = maxClientMessageSizeBytes;
            _log = log;
        }

        public async Task UpdateClientConfigurationAsync()
        {
            var message = new ClientConfigurationMessage
            {
                MaxClientMessageSizeBytes = _maxClientMessageSizeBytes,
            };

            _log.LogDebug($"Send client configuration with:");
            _log.LogDebug($"(MaxClientMessageSizeBytes='{message.MaxClientMessageSizeBytes}'");

            await _messageSender.SendAsync(message);
        }
    }
}
