using System;
using System.Configuration;
using Client.Core.Handling;
using Client.Core.Monitoring;
using Client.Data;
using Client.Data.Configuration;
using Client.MessageQueue;
using Client.MessageQueue.Builders;
using Client.MessageQueue.Receivers;
using Client.MessageQueue.Senders;
using Client.ScanService.Availability;
using Client.ScanService.Configuration;
using Client.ScanService.Configuration.External;
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

            RegisterExternalConfiguration(provider, config);

            var resourceMonitor = provider.GetService<IResourceMonitor>();
            resourceMonitor.StartMonitoring(ServiceConfigManager.GetScanFoldersConfig(config));

            var availabilityCheck = provider.GetService<IAvailabilityCheck>();
            availabilityCheck.StartChecks();
        }

        private static IServiceProvider ConfigureServices(ServiceConfig config)
        {
            ConfigureLogging(config.LogFilePath);

            var services = new ServiceCollection();
            services.AddSingleton(config);

            services.AddDbContext<IScanServiceContext, ScanServiceContext>(
                ConfigureDbContext);

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
                    () => config.MessageMaxSizeBytes));

            services.AddTransient<IAvailabilityCheck>(
                provider => new AvailabilityCheck(
                    new ServiceBusQueueMessageSender(
                        config.MonitoringQueueConnectionString,
                        config.MonitoringQueueName),
                    new ScanServiceContext(GetDbContextOptions()), 
                    config.ClientId,
                    config.AvailabilityCheckFrequencySeconds));

            services.AddTransient<IMessageReceiver>(
                provider => new ServiceBusSubscriptionMessageReceiver(
                    config.ConfigTopicConnectionString,
                    config.ConfigTopicName,
                    config.ConfigTopicSubscriptionName));

            services.AddTransient<IExternalConfigurationProvider, ExternalConfigurationProvider>();

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

        private static void RegisterExternalConfiguration(
            IServiceProvider provider,
            ServiceConfig config)
        {
            var externalProvider = provider.GetService<IExternalConfigurationProvider>();

            externalProvider.SyncExternalConfiguration(config);
        }

        private static DbContextOptions GetDbContextOptions()
        {
            var builder = new DbContextOptionsBuilder();
            ConfigureDbContext(builder);

            return builder.Options;
        }

        private static void ConfigureDbContext(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite(ConfigurationManager.ConnectionStrings["scan-service"].ConnectionString);
        }
    }
}
