using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Newtonsoft.Json;
using Service.MessageQueue.Messages.Base;

namespace Service.MessageQueue.Senders
{
    public class ServiceBusTopicMessageSender : IMessageSender
    {
        private const string MessageTypeKey = "MessageType";

        private readonly ISenderClient _senderClient;

        public ServiceBusTopicMessageSender(string connectionString, string topicName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentNullException(nameof(topicName));
            }

            _senderClient = new TopicClient(connectionString, topicName);
        }

        public async Task SendAsync(MessageBase message)
        {
            var json = SerializeMessage(message);
            var serviceBusMessage = new Message(Encoding.UTF8.GetBytes(json));

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
