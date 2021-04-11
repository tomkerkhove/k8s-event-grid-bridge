using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Arcus.EventGrid.Publishing;
using Arcus.EventGrid.Publishing.Interfaces;
using Kubernetes.EventGrid.Core.CloudEvents;
using Kubernetes.EventGrid.Core.CloudEvents.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes;
using Kubernetes.EventGrid.Core.Kubernetes.Interfaces;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

[assembly: FunctionsStartup(typeof(Kubernetes.EventGrid.Bridge.Host.Startup))]
namespace Kubernetes.EventGrid.Bridge.Host
{
    public class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            builder.ConfigurationBuilder.AddEnvironmentVariables("EventBridge_");
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = GetConfiguration(builder);

            AddDependencies(builder, configuration);

            ConfigureLogging(builder, configuration);
        }

        private void AddDependencies(IFunctionsHostBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddTransient<IKubernetesEventParser, KubernetesEventParser>();
            builder.Services.AddTransient<ICloudEventFactory, CloudEventFactory>();
            
            AddEventGridPublisher(builder, configuration);
        }

        private static void AddEventGridPublisher(IFunctionsHostBuilder builder, IConfiguration configuration)
        {
            var topicEndpoint = configuration["EventGrid_Topic_Uri"];
            var topicKey = configuration["EventGrid_Topic_Key"];

            var eventGridPublisher = EventGridPublisherBuilder
                .ForTopic(topicEndpoint)
                .UsingAuthenticationKey(topicKey)
                .Build();

            builder.Services.AddTransient<IEventGridPublisher>(provider => eventGridPublisher);
        }

        private static void ConfigureLogging(IFunctionsHostBuilder builder, IConfiguration configuration)
        {
            var instrumentationKey = configuration.GetValue<string>("APPINSIGHTS_INSTRUMENTATIONKEY");

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithComponentName("Kubernetes Event Grid    Bridge")
                .Enrich.WithVersion()
                .WriteTo.Console();

            if (string.IsNullOrWhiteSpace(instrumentationKey) == false)
            {
                loggerConfiguration = loggerConfiguration.WriteTo.AzureApplicationInsights(instrumentationKey);
            }
                
            var logger = loggerConfiguration.CreateLogger();

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProvidersExceptFunctionProviders();
                loggingBuilder.AddSerilog(logger);
            });
        }

        private static IConfiguration GetConfiguration(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            return configuration;
        }
    }
}
