﻿using System;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using Newtonsoft.Json;

namespace Kubernetes.EventBridge.Host.Kubernetes
{
    public class KubernetesEventWatcher
    {
        private readonly IConfiguration _configuration;
        private readonly KubernetesClientConfiguration _kubernetesConfiguration;
        private readonly ILogger _logger;
        private Watcher<V1Event> _watch;

        public KubernetesEventWatcher(ILogger logger, IConfiguration configuration)
        {
            _kubernetesConfiguration = KubernetesClientConfiguration.BuildConfigFromConfigFile();

            _logger = logger;
            _configuration = configuration;
        }

        public async Task Start(CancellationToken cancellationToken)
        {
            var kubernetesClient = new k8s.Kubernetes(_kubernetesConfiguration);
            var kubernetesNamespace = _configuration.GetValue<string>("NAMESPACE");
            HttpOperationResponse<V1EventList> eventsPerNamespace = await kubernetesClient.ListNamespacedEventWithHttpMessagesAsync(kubernetesNamespace, watch: true, cancellationToken: cancellationToken);

            _watch = eventsPerNamespace.Watch<V1Event>(HandleKubernetesEvent);
        }

        public Task Stop()
        {
            _watch.Dispose();

            return Task.CompletedTask;
        }

        private void HandleKubernetesEvent(WatchEventType type, V1Event kubernetesEvent)
        {
            var rawEvent = JsonConvert.SerializeObject(kubernetesEvent);
            _logger.LogInformation($"{DateTimeOffset.UtcNow:s} - {rawEvent}");
        }
    }
}