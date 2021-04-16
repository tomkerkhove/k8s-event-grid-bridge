using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes.Parsers;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public class ClusterAutoscalerEventConverter : EventConverter, IEventConverter
    {
        private const string EventSource = "http://kubernetes/autoscaling/cluster-autoscaler";

        public IKubernetesEvent ConvertFromNativeEvent(JToken parsedPayload)
        {
            var eventReason = parsedPayload["reason"]?.ToString()?.ToLower();
            switch (eventReason)
            {
                // TODO: Cluster node group is at max capacity
                // case "nottriggerscaleup":
                case "triggeredscaleup":
                    return ConvertClusterScalingOut(parsedPayload);
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }

        private IKubernetesEvent ConvertClusterScalingOut(JToken parsedPayload)
        {
            var nodeGroupResizeInformation = ClusterAutoscalerEventParser.ParseForClusterScalingOut(parsedPayload["message"]?.ToString());
            var scaleOutPayload = new ClusterAutoscalerScaleEventPayload
            {
                NodeGroup = nodeGroupResizeInformation
            };

            return CreateKubernetesEvent(KubernetesEventType.ClusterAutoscalerScaleOut, EventSource, $"/node-groups/{nodeGroupResizeInformation.Name}", scaleOutPayload, parsedPayload);
        }
    }
}
