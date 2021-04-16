namespace Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler
{
    public class ClusterAutoscalerScaleEventPayload
    {
        public NodeGroupResizeInfo NodeGroup { get; set; }
    }
}
