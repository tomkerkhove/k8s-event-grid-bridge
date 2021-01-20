using System.Threading.Tasks;
using Arcus.EventGrid.Publishing.Interfaces;
using GuardNet;
using Kubernetes.EventGrid.Core.CloudEvents.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventGrid.Bridge.Host
{
    public class KubernetesEventExporterFunction
    {
        private readonly ILogger<KubernetesEventExporterFunction> _logger;
        private readonly ICloudEventFactory _cloudEventFactory;
        private readonly IEventGridPublisher _eventGridPublisher;

        public KubernetesEventExporterFunction(ICloudEventFactory cloudEventFactory, IEventGridPublisher eventGridPublisher, ILogger<KubernetesEventExporterFunction> logger)
        {
            Guard.NotNull(eventGridPublisher, nameof(eventGridPublisher));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _cloudEventFactory = cloudEventFactory;
            _eventGridPublisher = eventGridPublisher;
        }

        [FunctionName("kubernetes-event-exporter")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, Route = "kubernetes/events/forward")] HttpRequest request)
        {
            var payload = await request.ReadAsStringAsync();
            _logger.LogInformation($"Kubernetes event received: {payload}");

            var cloudEvent = _cloudEventFactory.CreateFromRawKubernetesEvent(payload);
            await _eventGridPublisher.PublishAsync(cloudEvent);

            ILoggerExtensions.LogMetric(_logger, "Kubernetes Event Published", 1);
        }
    }
}
