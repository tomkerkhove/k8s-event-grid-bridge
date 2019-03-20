using System;
using System.Net.Mime;
using CloudNative.CloudEvents;
using GuardNet;
using k8s.Models;

namespace Kubernetes.EventBridge.Host.CloudEvents
{
    public class CloudEventsSchematizer
    {
        /// <summary>
        ///     Generates a Cloud Event for a Kubernetes Event (v1 schema)
        /// </summary>
        /// <param name="kubernetesEvent">Event that occured in Kubernetes cluster</param>
        public static CloudEvent GenerateFromKubernetesEvent(V1Event kubernetesEvent)
        {
            Guard.NotNull(kubernetesEvent, nameof(kubernetesEvent));

            var eventType = "Kubernetes.Event";
            var eventSource = new Uri(uriString: "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-bridge/providers/Microsoft.EventGrid/topics/k8s-event-bridge#k8s-event-bridge", uriKind: UriKind.Relative);
            var eventId = kubernetesEvent.Metadata.Uid ?? Guid.NewGuid().ToString();
            var eventTime = kubernetesEvent.LastTimestamp ?? DateTime.UtcNow;

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V0_1, eventType, eventSource, eventId, eventTime)
            {
                ContentType = new ContentType(contentType: "application/json"),
                Data = kubernetesEvent
            };

            return cloudEvent;
        }
    }
}