using System;
using System.Threading.Tasks;
using Client.MessageQueue.Extensions;
using Client.MessageQueue.Messages.Base;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;

namespace Client.MessageQueue.Receivers
{
    public class ServiceBusSubscriptionMessageReceiver : IMessageReceiver
    {
        private readonly IReceiverClient _receiverClient;

        public ServiceBusSubscriptionMessageReceiver(
            string connectionString,
            string topicName,
            string subscriptionName)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(connectionString));
            }

            if (string.IsNullOrEmpty(topicName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(topicName));
            }

            if (string.IsNullOrEmpty(subscriptionName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(subscriptionName));
            }

            _receiverClient = new SubscriptionClient(connectionString, topicName, subscriptionName);
        }

        public void RegisterHandler<TMessage>(Func<TMessage, Task> handler, Action<Exception> exceptionHandler)
            where TMessage : MessageBase
        {
            _receiverClient.RegisterMessageHandler(
                async (serviceBusMessage, token) =>
                {
                    var message = serviceBusMessage.Read();
                    if (message is TMessage handlerMessage)
                    {
                        await handler(handlerMessage);
                    }
                },
                (args) =>
                {
                    exceptionHandler(args.Exception);
                    return Task.CompletedTask;
                });
        }
    }
}
