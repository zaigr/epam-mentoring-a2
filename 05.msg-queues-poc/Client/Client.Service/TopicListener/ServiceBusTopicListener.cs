using System;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Client.Service.TopicListener
{
    public class ServiceBusTopicListener : ITopicListener
    {
        private readonly IReceiverClient _receiverClient;

        public ServiceBusTopicListener(string connectionString, string topicPath, string subscriptionName)
        {
            _receiverClient = new SubscriptionClient(connectionString, topicPath, subscriptionName);
        }

        public void StartListener(Action<Message> handler)
        {
            _receiverClient.RegisterMessageHandler(
                (message, token) =>
                {
                    handler(message);
                    return Task.CompletedTask;
                },
                (eventArgs) => Task.CompletedTask);
        }
    }
}
