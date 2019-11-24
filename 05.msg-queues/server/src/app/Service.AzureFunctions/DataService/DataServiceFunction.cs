using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Service.MessageQueue.Extensions;
using Service.MessageQueue.Processing;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace Service.AzureFunctions.DataService
{
    public static class DataServiceFunction
    {
        [FunctionName("DataService")]
        public static async Task Run([ServiceBusTrigger("%DataQueueName%",
            Connection = "ConnectionStrings:DataQueue", IsSessionsEnabled = true)]
            Message message,
            [Inject]ILogger log,
            [Inject]IMessageProcessor messageProcessor)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message (sessionId='{message.SessionId}').");

                var clientMessage = message.Read();

                await messageProcessor.HandleAsync(clientMessage);
            }
            catch
            {
                log.LogError($"Function 'DataService' failed with exception while processing message (sessionId='{message.SessionId}').");
                throw;
            }
        }
    }
}
