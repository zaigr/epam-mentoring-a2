using System;
using System.Configuration;
using System.Threading.Tasks;
using Client.Core.Handling;
using Client.Core.Monitoring;
using Client.Data;
using Client.Data.Configuration;
using Client.MessageQueue.Builders;
using Client.MessageQueue.Senders;
using Client.ScanService.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Client.ScanService
{
    internal static class ServiceLocator
    {
        public static void Start()
        {
            var config = ServiceConfigManager.GetConfig();
            var provider = ConfigureServices(config);

            var resourceMonitor = provider.GetService<IResourceMonitor>();

            Task.WaitAll(
                resourceMonitor.StartMonitoring(ServiceConfigManager.GetScanFoldersConfig(config)));
        }

        private static IServiceProvider ConfigureServices(ServiceConfig config)
        {
            ConfigureLogging(config.LogFilePath);

            var services = new ServiceCollection();
            services.AddSingleton(config);

            services.AddDbContext<IScanServiceContext, ScanServiceContext>(
                builder =>
                {
                    builder.UseSqlite(ConfigurationManager.ConnectionStrings["scan-service"].ConnectionString);
                });

            services.AddTransient<IResourceMonitor, FolderMonitor>(
                provider =>
                {
                    var monitor = new FolderMonitor(provider.GetService<IScanServiceContext>(), config.FolderScanFrequencySeconds);

                    var handlers = provider.GetServices<IResourceMonitorEventHandler>();
                    foreach (var handler in handlers)
                    {
                        monitor.ResourceAdded += handler.ResourceAddedEventHandler;
                        monitor.ResourceChanged += handler.ResourceChangedEventHandler;
                    }

                    return monitor;
                });

            services.AddTransient<IResourceMonitorEventHandler, ResourceStreamingHandler>(
                provider => new ResourceStreamingHandler(
                    new MessageSequenceBuilder(),
                    new ServiceBusQueueMessageSender(config.DataQueueConnectionString, config.DataQueueName),
                    config.MessageMaxSizeBytes));

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
