
namespace Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler
{
    public class NodeGroupResizeInfo
    {
        public string Name { get; set; }
        public NewNodeGroupSizeInfo SizeInfo { get; set; }
    }
}
