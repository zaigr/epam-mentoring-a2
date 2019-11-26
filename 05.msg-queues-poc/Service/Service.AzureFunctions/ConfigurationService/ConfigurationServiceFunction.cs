using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Service.AzureFunctions.ConfigurationService.TopicSender;

namespace Service.AzureFunctions.ConfigurationService
{
    public class ConfigurationServiceFunction
    {
        private const string TopicConnectionString = "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=6U0k6rYLz6GqwxKFTFDmNPn4E8pgFSQIx1ljTTJ8oAI=";
        private const string TopicName = "configuration-topic";

        [FunctionName("ConfigurationService")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var sender = new ServiceBusTopicSender(TopicConnectionString, TopicName);

            var message = new Message(Encoding.UTF8.GetBytes("New configuration"));
            await sender.SendAsync(message);

            log.LogInformation("New configuration sent.");
        }
    }
}
