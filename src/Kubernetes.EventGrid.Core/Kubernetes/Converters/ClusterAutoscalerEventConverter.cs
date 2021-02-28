using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler;
using Kubernetes.EventGrid.Core.Kubernetes.Events;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public class ClusterAutoscalerEventConverter : EventConverter
    {
        public IKubernetesEvent ConvertFromNativeEvent(JToken parsedPayload)
        {
            var eventReason = parsedPayload["reason"]?.ToString()?.ToLower();
            switch (eventReason)
            {
                case "scaledown":
                    var scaleInPayload = new ClusterAutoscalerScaleEventPayload();
                    return new KubernetesEvent(KubernetesEventType.ClusterAutoscalerScaleIn, scaleInPayload);
                case "triggeredscaleup":
                    var scaleOutPayload = new ClusterAutoscalerScaleEventPayload();
                    return new KubernetesEvent(KubernetesEventType.ClusterAutoscalerScaleOut, scaleOutPayload);
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }
    }
}
