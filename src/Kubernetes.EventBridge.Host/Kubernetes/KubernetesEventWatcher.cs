using System;
using System.Threading;
using System.Threading.Tasks;
using GuardNet;
using k8s;
using k8s.Models;
using Kubernetes.EventBridge.Host.CloudEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Kubernetes.EventBridge.Host.Kubernetes
{
    public class KubernetesEventWatcher
    {
        private readonly CloudEventsPublisher _cloudEventsPublisher;
        private readonly CloudEventsSchematizer _cloudEventsSchematizer;
        private readonly IConfiguration _configuration;
        private readonly KubernetesClientConfiguration _kubernetesConfiguration;
        private readonly ILogger _logger;
        private Watcher<V1Event> _watch;

        public KubernetesEventWatcher(string topicEndpointUri, string eventSource, ILogger logger, IConfiguration configuration)
        {
            Guard.NotNullOrEmpty(topicEndpointUri, nameof(topicEndpointUri));
            Guard.NotNullOrEmpty(eventSource, nameof(eventSource));

            _kubernetesConfiguration = KubernetesClientConfiguration.BuildConfigFromConfigFile();

            _logger = logger;
            _configuration = configuration;

            _cloudEventsPublisher = new CloudEventsPublisher(topicEndpointUri, logger);
            _cloudEventsSchematizer = new CloudEventsSchematizer(eventSource);
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            var kubernetesClient = new k8s.Kubernetes(_kubernetesConfiguration);
            var kubernetesNamespace = _configuration.GetValue<string>(key: "NAMESPACE");
            HttpOperationResponse<V1EventList> eventsPerNamespace = await kubernetesClient.ListNamespacedEventWithHttpMessagesAsync(kubernetesNamespace, watch: true, cancellationToken: cancellationToken);

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
            var rawCloudEvent = JsonConvert.SerializeObject(cloudEvent);
            _logger.LogInformation($"{DateTimeOffset.UtcNow:s} - {rawCloudEvent}");
        }
    }
}