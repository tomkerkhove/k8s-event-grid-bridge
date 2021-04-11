using System;
using GuardNet;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;

namespace Kubernetes.EventGrid.Core.Kubernetes.Events
{
    public class KubernetesEvent : IKubernetesEvent
    {
        public KubernetesEventType Type { get; }
        public object Payload { get; set; }
        public Uri? Source { get; set; }
        public string Subject { get; set; }

        public KubernetesEvent(KubernetesEventType eventType, object payload)
        {
            Guard.For<ArgumentException>(() => eventType == KubernetesEventType.Unspecified, "No event type was provided");
            Guard.NotNull(nameof(payload), nameof(payload));

            Type = eventType;
            Payload = payload;
        }
    }
}
