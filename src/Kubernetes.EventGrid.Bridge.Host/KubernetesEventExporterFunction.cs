using System.Collections.Generic;
using System.Threading.Tasks;
using Arcus.EventGrid.Publishing.Interfaces;
using CloudNative.CloudEvents;
using GuardNet;
using Kubernetes.EventGrid.Core.CloudEvents.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, Route = "kubernetes/events/forward")] HttpRequest request)
        {
            var cloudEvent = await ConvertRequestToCloudEvent(request);
            
            await PublishEventAsync(cloudEvent);
            
            MeasureEventPublished();
            
            request.HttpContext.Response.Headers.TryAdd("X-Event-Id", cloudEvent.Id);
            return new OkResult();
        }

        private async Task<CloudEvent> ConvertRequestToCloudEvent(HttpRequest request)
        {
            var payload = await request.ReadAsStringAsync();
            _logger.LogInformation($"Kubernetes event received: {payload}");

            var cloudEvent = _cloudEventFactory.CreateFromRawKubernetesEvent(payload);
            return cloudEvent;
        }

        private async Task PublishEventAsync(CloudEvent cloudEvent)
        {
            await _eventGridPublisher.PublishAsync(cloudEvent);
        }

        private void MeasureEventPublished()
        {
            ILoggerExtensions.LogMetric(_logger, "Kubernetes Event Published", 1);
        }
    }
}
