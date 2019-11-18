using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Service.AzureFunctions.DataService
{
    public class DataServiceFunction
    {
        [FunctionName("DataService")]
        public void Run([ServiceBusTrigger("%DataQueueName%", Connection = "QueueConnectionString", IsSessionsEnabled = true)]Message message, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {Encoding.UTF8.GetString(message.Body)}");
            log.LogInformation($"SessionId: '{message.SessionId}'");
        }
    }
}
