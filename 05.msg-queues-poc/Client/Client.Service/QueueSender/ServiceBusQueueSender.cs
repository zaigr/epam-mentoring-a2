using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Client.Service.QueueSender
{
    public class ServiceBusQueueSender : IQueueSender
    {
        private readonly ISenderClient _senderClient;

        public ServiceBusQueueSender(string connectionString, string entityPath)
        {
            _senderClient = new QueueClient(connectionString, entityPath);
        }

        public async Task SendAsync(Message message)
        {
            await _senderClient.SendAsync(message);
        }
    }
}
