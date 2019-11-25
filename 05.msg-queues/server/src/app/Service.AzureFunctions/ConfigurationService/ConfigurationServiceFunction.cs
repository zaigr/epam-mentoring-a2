using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Service.AzureFunctions.ConfigurationService.Services;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;

namespace Service.AzureFunctions.ConfigurationService
{
    public static class ConfigurationServiceFunction
    {
        [FunctionName("ConfigurationService")]
        public static async Task Run(
            [TimerTrigger("%ConfigurationServiceTriggerTime%")]TimerInfo myTimer,
            [Inject]ILogger log,
            [Inject]IClientConfigurationService configurationService)
        {
            log.LogInformation($"'ConfigurationService' function executed at: {DateTime.Now}");

            await configurationService.UpdateClientConfigurationAsync();
        }
    }
}
