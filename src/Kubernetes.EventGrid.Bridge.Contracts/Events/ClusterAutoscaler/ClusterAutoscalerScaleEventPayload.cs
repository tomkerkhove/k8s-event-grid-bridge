namespace Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler
{
    public class ClusterAutoscalerScaleEventPayload
    {
        public NodeGroupResizeInformation NodeGroup { get; set; }
    }
}
