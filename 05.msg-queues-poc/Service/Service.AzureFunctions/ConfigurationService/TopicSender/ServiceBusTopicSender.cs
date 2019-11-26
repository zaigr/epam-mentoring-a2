using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System.Threading.Tasks;

namespace Service.AzureFunctions.ConfigurationService.TopicSender
{
    public class ServiceBusTopicSender : ITopicSender
    {
        private readonly ISenderClient _senderClient;

        public ServiceBusTopicSender(string connectionString, string topicName)
        {
            _senderClient = new TopicClient(connectionString, topicName);
        }

        public async Task SendAsync(Message message)
        {
            await _senderClient.SendAsync(message);
        }
    }
}
