using System;
using System.Net.Mime;
using CloudNative.CloudEvents;
using GuardNet;
using Kubernetes.EventGrid.Bridge.Contracts.Extensions;
using Kubernetes.EventGrid.Core.CloudEvents.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes.Interfaces;

namespace Kubernetes.EventGrid.Core.CloudEvents
{
    public class CloudEventFactory : ICloudEventFactory
    {
        private readonly IKubernetesEventParser _kubernetesEventParser;
        private readonly IKubernetesClusterInfoProvider _kubernetesClusterInfoProvider;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="kubernetesEventParser">Converter to convert raw Kubernetes events into user-friendly events</param>
        /// <param name="kubernetesClusterInfoProvider">Provider for getting Kubernetes cluster information</param>
        public CloudEventFactory(IKubernetesEventParser kubernetesEventParser, IKubernetesClusterInfoProvider kubernetesClusterInfoProvider)
        {
            Guard.NotNull(kubernetesClusterInfoProvider, nameof(kubernetesClusterInfoProvider));
            Guard.NotNull(kubernetesClusterInfoProvider, nameof(kubernetesClusterInfoProvider));

            _kubernetesEventParser = kubernetesEventParser;
            _kubernetesClusterInfoProvider = kubernetesClusterInfoProvider;
        }

        /// <summary>
        ///     Creates a CloudEvent for a raw Kubernetes event
        /// </summary>
        /// <param name="payload">Raw Kubernetes event</param>
        public virtual (CloudEvent Event, KubernetesEventContext KubernetesEventContext) CreateFromRawKubernetesEvent(string payload)
        {
            var kubernetesEvent = _kubernetesEventParser.ParseFromRawNativeEvent(payload);
            
            var eventType = kubernetesEvent.Type.GetDescription();
            var source = kubernetesEvent.Source ?? new Uri("http://kubernetes");
            var subject = ComposeEventSubject(kubernetesEvent);
            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0, eventType, source, subject: subject)
            {
                DataContentType = new ContentType("application/json"),
                Data = kubernetesEvent.Payload
            };

            return (cloudEvent, new KubernetesEventContext(kubernetesEvent.Namespace));
        }

        private string ComposeEventSubject(IKubernetesEvent kubernetesEvent)
        {
            var clusterName = _kubernetesClusterInfoProvider.GetClusterName();
            var subject = kubernetesEvent.Subject ?? string.Empty;

            // Remove leading /, if present
            if (subject.StartsWith("/"))
            {
                subject = subject.Remove(0, 1);
            }

            // Always suffix with cluster name
            return $"/{clusterName}/{subject}";
        }
    }
}