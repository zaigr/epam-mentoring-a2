﻿using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using Service.AzureFunctions;
using Service.AzureFunctions.AvailabilityService.MessageHandlers;
using Service.AzureFunctions.ConfigurationService.Services;
using Service.AzureFunctions.DataService.MessageHandlers;
using Service.Data;
using Service.Data.Configuration;
using Service.MessageQueue.Messages;
using Service.MessageQueue.Processing;
using Service.MessageQueue.Senders;
using Service.Storage;
using Willezone.Azure.WebJobs.Extensions.DependencyInjection;
using ILogger = Microsoft.Extensions.Logging.ILogger;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Service.AzureFunctions
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var configBuilder = new ConfigurationBuilder()
                .AddEnvironmentVariables();
            builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), configBuilder.Build()));

            builder.AddDependencyInjection<ServiceProviderBuilder>();
        }
    }

    internal class ServiceProviderBuilder : IServiceProviderBuilder
    {
        private readonly IConfiguration _configuration;

        public ServiceProviderBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IServiceProvider Build()
        {
            var services = new ServiceCollection();

            services.AddSingleton(provider =>
            {
                var factory = provider.GetRequiredService<ILoggerFactory>();
                return factory.AddSerilog(Log.Logger).CreateLogger("Logger");
            });

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            services.AddDbContext<IAvailabilityServiceContext, AvailabilityServiceContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("MonitoringDatabase")));

            services.AddTransient<IStorageClient, AzureStorageClient>(
                provider => new AzureStorageClient(
                    _configuration.GetConnectionString("BlobStorage"),
                    _configuration["BlobContainerName"]));

            services.AddTransient<IClientConfigurationService>(
                provider => new ClientConfigurationService(
                    new ServiceBusTopicMessageSender(
                        _configuration.GetConnectionString("ConfigurationTopic"),
                        _configuration["ConfigurationTopicName"]),
                    provider.GetService<ILogger>(),
                    _configuration.GetValue<int>("MaxClientMessageSizeBytes")));
            
            services.AddScoped<IMessageProcessor, MessageProcessor>();
            services.AddTransient<IMessageHandler<FileContentMessage>, FileContentMessageHandler>();
            services.AddTransient<IMessageHandler<ClientAvailabilityMessage>, ClientAvailabilityMessageHandler>();

            return services.BuildServiceProvider();
        }
    }
}
