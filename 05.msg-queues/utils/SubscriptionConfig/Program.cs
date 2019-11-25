using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace SubscriptionConfig
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var connectionString = "Endpoint=sb://etl-sb-poc.servicebus.windows.net/;SharedAccessKeyName=RootPolicy;SharedAccessKey=6U0k6rYLz6GqwxKFTFDmNPn4E8pgFSQIx1ljTTJ8oAI=";
            var topicName = "configuration-topic";
            var subscriptionName = "client-service-configuration";

            var client = new SubscriptionClient(connectionString, topicName, subscriptionName);

            try
            {
                var rules = await client.GetRulesAsync();
                if (rules.All(r => r.Name != "client-service-configuration-filter"))
                {
                    var filter = new SqlFilter("MessageType IN ('ClientConfigurationMessage')");
                    await client.AddRuleAsync("client-service-configuration-filter", filter);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
