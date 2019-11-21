using System;
using System.Configuration;
using System.Threading.Tasks;
using Client.Core.Monitoring;
using Client.Data;
using Client.Data.Configuration;
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
                    monitor.ResourceAdded += (sender, args) => Console.WriteLine($"{args.ResourcePath}\\{args.ResourceName} added.");
                    monitor.ResourceChanged += (sender, args) => Console.WriteLine($"{args.ResourcePath}\\{args.ResourceName} changed.");

                    return monitor;
                });

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
