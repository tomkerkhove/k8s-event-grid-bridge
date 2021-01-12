using System;
using System.Threading.Tasks;
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
            _logger.LogInformation($"Kubernetes event received: {payload}");

            var cloudEvent = ConvertToCloudEvent(payload);
            await _eventGridPublisher.PublishAsync(cloudEvent);

            ILoggerExtensions.LogMetric(_logger, "Kubernetes Event Published", 1);
        }

        private static CloudEvent ConvertToCloudEvent(string payload)
        {
            var @event = JsonConvert.DeserializeObject<object>(payload);

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0, "Kubernetes.Events.Raw", new Uri("http://kubernetes"))
            {
                DataContentType = new ContentType("application/json"), 
                Data = @event
            };

            return cloudEvent;
        }
    }
}
