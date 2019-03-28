using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using k8s;
using k8s.Models;
using Kubernetes.EventBridge.Host.BackgroundServices;
using Kubernetes.EventBridge.Host.CloudEvents;
using Kubernetes.EventBridge.Host.Configuration;
using Kubernetes.EventBridge.Host.Configuration.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace Kubernetes.EventBridge.Host.Kubernetes
{
    public class KubernetesEventWatcher
    {
        private readonly CloudEventsPublisher _cloudEventsPublisher;
        private readonly CloudEventsSchematizer _cloudEventsSchematizer;
        private readonly KubernetesClientConfiguration _kubernetesConfiguration;
        private readonly ILogger _logger;
        private Watcher<V1Event> _watch;

        public KubernetesEventWatcher(string kubernetesNamespace, EventTopicConfiguration eventTopic, string eventSource, ILogger logger)
        {
            Guard.NotNullOrEmpty(kubernetesNamespace, nameof(kubernetesNamespace));
            Guard.NotNull(eventTopic, nameof(eventTopic));
            Guard.NotNullOrEmpty(eventSource, nameof(eventSource));

            KubernetesNamespace = kubernetesNamespace;

            _kubernetesConfiguration = KubernetesClientConfiguration.BuildConfigFromConfigFile();

            _logger = logger;

            _cloudEventsPublisher = new CloudEventsPublisher(eventTopic, logger);
            _cloudEventsSchematizer = new CloudEventsSchematizer(eventSource);
        }

        public string KubernetesNamespace { get; }

        public async Task Start(CancellationToken cancellationToken)
        {
            var kubernetesClient = new k8s.Kubernetes(_kubernetesConfiguration);
            HttpOperationResponse<V1EventList> eventsPerNamespace = await kubernetesClient.ListNamespacedEventWithHttpMessagesAsync(KubernetesNamespace, watch: true, cancellationToken: cancellationToken);

            _watch = eventsPerNamespace.Watch<V1Event>(async (type, kubernetesEvent) => await HandleKubernetesEvent(type, kubernetesEvent));
        }

        public Task Stop()
        {
            _watch.Dispose();

            return Task.CompletedTask;
        }

        private async Task HandleKubernetesEvent(WatchEventType type, V1Event kubernetesEvent)
        {
            var cloudEvent = _cloudEventsSchematizer.GenerateFromKubernetesEvent(kubernetesEvent);
            await _cloudEventsPublisher.Publish(cloudEvent);
        }
    }
}