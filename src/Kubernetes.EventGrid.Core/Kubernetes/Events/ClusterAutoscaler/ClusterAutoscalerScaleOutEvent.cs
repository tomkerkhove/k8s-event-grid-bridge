using Kubernetes.EventGrid.Core.Kubernetes.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;

namespace Kubernetes.EventGrid.Core.Kubernetes.Events.ClusterAutoscaler
{
    public class ClusterAutoscalerScaleOutEvent : KubernetesEvent, IKubernetesEvent
    {
        public KubernetesEventType Type { get; } = KubernetesEventType.ClusterAutoscalerScaleOut;
    }
}
