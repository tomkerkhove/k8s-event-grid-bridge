using System.ComponentModel;

namespace Kubernetes.EventGrid.Core.Kubernetes.Enums
{
    public enum KubernetesEventType
    {
        [Description("Kubernetes.Events.Raw")]
        Raw,
        [Description("Kubernetes.Autoscaling.ClusterAutoscaler.ScaleIn")]
        ClusterAutoscalerScaleIn,
        [Description("Kubernetes.Autoscaling.ClusterAutoscaler.ScaleOut")]
        ClusterAutoscalerScaleOut,
    }
}
