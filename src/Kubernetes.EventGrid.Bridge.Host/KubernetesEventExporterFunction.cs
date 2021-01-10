using System;
using System.Threading.Tasks;
using Arcus.EventGrid.Publishing.Interfaces;
using CloudNative.CloudEvents;
using GuardNet;
using k8s.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ContentType = System.Net.Mime.ContentType;

namespace Kubernetes.EventGrid.Bridge.Host
{
    public class KubernetesEventExporterFunction
    {
        private readonly ILogger<KubernetesEventExporterFunction> _logger;
        private readonly IEventGridPublisher _eventGridPublisher;

        public KubernetesEventExporterFunction(IEventGridPublisher eventGridPublisher, ILogger<KubernetesEventExporterFunction> logger)
        {
            Guard.NotNull(eventGridPublisher, nameof(eventGridPublisher));
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;
            _eventGridPublisher = eventGridPublisher;
        }

        [FunctionName("kubernetes-event-exporter")]
        public async Task Run([HttpTrigger(AuthorizationLevel.Anonymous, Route = "kubernetes/events/forward")] HttpRequest request)
        {
            var payload = await request.ReadAsStringAsync();

            foreach (var header in request.Headers)
            {
                _logger.LogInformation($"Request header {header.Key}: {header.Value}");
            }
            
            _logger.LogInformation($"Kubernetes event received: {payload}");

            var cloudEvent = ConvertToCloudEvent(payload);
            await _eventGridPublisher.PublishAsync(cloudEvent);

            ILoggerExtensions.LogMetric(_logger, "Kubernetes Event Published", 1);
        }

        private static CloudEvent ConvertToCloudEvent(string payload)
        {
            var @event = JsonConvert.DeserializeObject<Eventsv1Event>(payload);
            var @event2 = JsonConvert.DeserializeObject<Corev1Event>(payload);
            var @event3 = JsonConvert.DeserializeObject<V1beta1Event>(payload);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0, "Kubernetes.Events.Generic", new Uri("http://temporary"))
            {
                DataContentType = new ContentType("application/json"), 
                Data = @event2
            };

            return cloudEvent;
        }
    }
}
