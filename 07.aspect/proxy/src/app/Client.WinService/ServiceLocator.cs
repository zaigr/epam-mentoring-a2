using System.Configuration;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Client.Core.Handling;
using Client.Core.Monitoring;
using Client.Data;
using Client.Data.Configuration;
using Client.MessageQueue;
using Client.MessageQueue.Builders;
using Client.MessageQueue.Receivers;
using Client.MessageQueue.Senders;
using Client.ScanService.AOP.Advices;
using Client.ScanService.Availability;
using Client.ScanService.Configuration;
using Client.ScanService.Configuration.External;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Client.ScanService
{
    internal static class ServiceLocator
    {
        public static void Start()
        {
            var config = ServiceConfigManager.GetConfig();
            var container = ConfigureServices(config);

            RegisterExternalConfiguration(container, config);

            var resourceMonitor = container.Resolve<IResourceMonitor>();
            resourceMonitor.StartMonitoring(ServiceConfigManager.GetScanFoldersConfig(config));

            var availabilityCheck = container.Resolve<IAvailabilityCheck>();
            availabilityCheck.StartChecks();
        }

        private static IWindsorContainer ConfigureServices(ServiceConfig config)
        {
            ConfigureLogging(config.LogFilePath);

            var container = new WindsorContainer();

            container.Register(new ComponentRegistration(config));

            return container;
        }

        private class ComponentRegistration : IRegistration
        {
            private readonly ServiceConfig _config;

            public ComponentRegistration(ServiceConfig config)
            {
                _config = config;
            }

            public void Register(IKernelInternal kernel)
            {
                kernel.Register(
                    Component
                        .For<ServiceConfig>()
                        .Instance(_config)
                        .LifeStyle.Singleton);

                kernel.Register(
                    Component.For<MethodCallLoggingAdvice>()
                        .ImplementedBy<MethodCallLoggingAdvice>());

                kernel.Register(
                    Component
                        .For<IScanServiceContext>()
                        .UsingFactoryMethod(ConfigureDbContext)
                        .LifeStyle.Transient);

                kernel.Register(
                    Component
                        .For<IResourceMonitor>()
                        .Interceptors<MethodCallLoggingAdvice>()
                        .UsingFactoryMethod(
                            k =>
                            {
                                var monitor = new FolderMonitor(
                                    k.Resolve<IScanServiceContext>(),
                                    _config.FolderScanFrequencySeconds);

                                var handlers = k.ResolveAll<IResourceMonitorEventHandler>();
                                foreach (var handler in handlers)
                                {
                                    monitor.ResourceAdded += handler.ResourceAddedEventHandler;
                                    monitor.ResourceChanged += handler.ResourceChangedEventHandler;
                                }

                                return monitor;
                            }));

                kernel.Register(
                    Component.For<IResourceMonitorEventHandler>()
                        .Interceptors<MethodCallLoggingAdvice>()
                        .UsingFactoryMethod(
                            _ => new ResourceStreamingHandler(
                                new MessageSequenceBuilder(),
                                new ServiceBusQueueMessageSender(
                                    _config.DataQueueConnectionString,
                                    _config.DataQueueName),
                                () => _config.MessageMaxSizeBytes))
                        .LifeStyle.Transient);

                kernel.Register(
                    Component.For<IAvailabilityCheck>()
                        .Interceptors<MethodCallLoggingAdvice>()
                        .UsingFactoryMethod(
                            _ => new AvailabilityCheck(
                                new ServiceBusQueueMessageSender(
                                    _config.MonitoringQueueConnectionString,
                                    _config.MonitoringQueueName),
                                ConfigureDbContext(),
                                _config.ClientId,
                                _config.AvailabilityCheckFrequencySeconds))
                        .LifeStyle.Transient);

                kernel.Register(
                    Component.For<IMessageReceiver>()
                        .Interceptors<MethodCallLoggingAdvice>()
                        .UsingFactoryMethod(
                            _ => new ServiceBusSubscriptionMessageReceiver(
                                _config.ConfigTopicConnectionString,
                                _config.ConfigTopicName,
                                _config.ConfigTopicSubscriptionName))
                        .LifeStyle.Transient);

                kernel.Register(
                    Component.For<IExternalConfigurationProvider>()
                        .ImplementedBy(typeof(ExternalConfigurationProvider))
                        .Interceptors<MethodCallLoggingAdvice>()
                        .LifeStyle.Transient);
            }
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
            IWindsorContainer provider,
            ServiceConfig config)
        {
            var externalProvider = provider.Resolve<IExternalConfigurationProvider>();

            externalProvider.SyncExternalConfiguration(config);
        }

        private static ScanServiceContext ConfigureDbContext()
        {
            var builder = new DbContextOptionsBuilder();
            builder.UseSqlite(ConfigurationManager.ConnectionStrings["scan-service"].ConnectionString);

            return new ScanServiceContext(builder.Options);
        }
    }
}
