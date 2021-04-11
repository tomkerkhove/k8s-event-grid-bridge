using System.ComponentModel;

namespace Kubernetes.EventGrid.Bridge.Contracts.Enums
{
    public enum KubernetesEventType
    {
        Unspecified,
        [Description("Kubernetes.Events.Raw")]
        Raw,
        [Description("Kubernetes.Autoscaling.ClusterAutoscaler.NodeGroup.ScaleOut")]
        ClusterAutoscalerScaleOut,
    }
}
