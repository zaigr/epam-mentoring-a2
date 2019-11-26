using System;
using System.Linq;
using System.Text;
using Client.Service.QueueSender;
using Microsoft.Azure.ServiceBus;
using Client.Service.TopicListener;

namespace Client.Service
{
    public static class Program
    {
        private const string QueueConnectionString = "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=LziGOa7UMlgc8yqvATrIgfWQd7RnxUJ9Hb6zViMA33Q=";
        private const string DataQueueName = "data-queue";

        private const string TopicConnectionString = "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=6U0k6rYLz6GqwxKFTFDmNPn4E8pgFSQIx1ljTTJ8oAI=";
        private const string ConfigurationTopicName = "configuration-topic";
        private const string ServiceConfigSubscriptionName = "client-service-configuration";

        static void Main(string[] args)
        {
            StartListener();

            StartSender();

            // StartBatchSender();
        }

        private static void StartListener()
        {
            var receiver = new ServiceBusTopicListener(TopicConnectionString, ConfigurationTopicName, ServiceConfigSubscriptionName);

            receiver.StartListener(
                message =>
                {
                    Console.WriteLine($"Message Received from topic '{ServiceConfigSubscriptionName}':");
                    Console.WriteLine(Encoding.UTF8.GetString(message.Body));
                });
        }

        private static void StartSender()
        {
            var sender = new ServiceBusQueueSender(QueueConnectionString, DataQueueName);

            while (true)
            {
                Console.WriteLine("Enter number of messages.");
                var messageCount = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

                var sessionId = Guid.NewGuid().ToString();
                var messages = Enumerable.Range(0, messageCount)
                    .Select(i => new Message(Encoding.UTF8.GetBytes($"Hello world {i}"))
                    {
                        SessionId = sessionId
                    });

                Console.WriteLine($"SessionId: {sessionId}");

                try
                {
                    foreach (var message in messages)
                    {
                        sender.SendAsync(message).GetAwaiter().GetResult();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
        private static void StartBatchSender()
        {
            
        }
    }
}
