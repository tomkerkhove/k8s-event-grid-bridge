using Kubernetes.EventGrid.Core.Kubernetes.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;

namespace Kubernetes.EventGrid.Core.Kubernetes.Events
{
    public class RawKubernetesEvent : KubernetesEvent, IKubernetesEvent
    {
        public KubernetesEventType Type => KubernetesEventType.Raw;
    }
}
