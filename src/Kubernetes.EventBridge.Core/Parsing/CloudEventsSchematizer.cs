using System;
using System.Net.Mime;
using CloudNative.CloudEvents;
using GuardNet;
using k8s.Models;

namespace Kubernetes.EventBridge.Core.Parsing
{
    public class CloudEventsSchematizer
    {
        private const string DefaultEventType = "Kubernetes.Event";

        /// <summary>
        /// </summary>
        /// <param name="eventSourceUri">The source of all events where the cluster is running</param>
        public CloudEventsSchematizer(string eventSourceUri)
        {
            Guard.NotNullOrEmpty(eventSourceUri, nameof(eventSourceUri));

            EventSource = new Uri(eventSourceUri, UriKind.Relative);
        }

        /// <summary>
        ///     Source of all events where the cluster is running
        /// </summary>
        public Uri EventSource { get; }

        /// <summary>
        ///     Generates a Cloud Event for a Kubernetes Event (v1 schema)
        /// </summary>
        /// <param name="kubernetesEvent">Event that occured in Kubernetes cluster</param>
        public CloudEvent GenerateFromKubernetesEvent(V1Event kubernetesEvent)
        {
            // TODO: Remove Kubernetes Client dependency
            Guard.NotNull(kubernetesEvent, nameof(kubernetesEvent));

            var eventId = kubernetesEvent.Metadata.Uid ?? Guid.NewGuid().ToString();
            var eventTime = kubernetesEvent.LastTimestamp ?? DateTime.UtcNow;

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.Default, DefaultEventType, EventSource, eventId, eventTime)
            {
                Data = kubernetesEvent,
                DataContentType = new ContentType(contentType: "application/json")
            };

            return cloudEvent;
        }
    }
}