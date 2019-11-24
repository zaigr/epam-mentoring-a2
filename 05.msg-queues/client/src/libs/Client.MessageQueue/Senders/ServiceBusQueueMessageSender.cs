using System.Text;
using System.Threading.Tasks;
using Client.MessageQueue.Messages.Base;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;

namespace Client.MessageQueue.Senders
{
    public class ServiceBusQueueMessageSender : IMessageSender
    {
        private const string MessageTypeKey = "MessageType";

        private readonly ISenderClient _senderClient;

        public ServiceBusQueueMessageSender(string connectionString, string queueName)
        {
            _senderClient = new QueueClient(connectionString, queueName);
        }

        public async Task SendAsync(MessageBase message)
        {
            var json = SerializeMessage(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(json))
            {
                SessionId = message.SessionId
            };

            serviceBusMessage.UserProperties.Add(MessageTypeKey, message.MessageType.ToString());

            await _senderClient.SendAsync(serviceBusMessage);
        }

        private string SerializeMessage(MessageBase message)
        {
            return JsonConvert.SerializeObject(
                message,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }
    }
}
