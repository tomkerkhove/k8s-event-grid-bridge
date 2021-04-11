
namespace Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler
{
    public class NewNodeGroupSizeInfo
    {
        public int New { get; set; }
        public int Old { get; set; }
        public int Maximum { get; set; }
    }
}
