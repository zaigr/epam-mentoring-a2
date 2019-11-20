using System;
using Client.ScanService.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Client.ScanService
{
    internal static class ServiceLocator
    {
        public static void Start()
        {
            var provider = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var config = ServiceConfigManager.GetConfig();
            ConfigureLogging(config.LogFilePath);

            var services = new ServiceCollection();
            services.AddSingleton(config);

            return services.BuildServiceProvider();
        }

        private static void ConfigureLogging(string logFilePath)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File(logFilePath)
                .CreateLogger();
        }
    }
}
