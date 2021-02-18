using Kubernetes.EventGrid.Core.Kubernetes.Events;
using Kubernetes.EventGrid.Core.Kubernetes.Events.ClusterAutoscaler;
using Kubernetes.EventGrid.Core.Kubernetes.Events.ClusterAutoscaler.Contracts;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes
{
    public class KubernetesEventParser : IKubernetesEventParser
    {
        /// <summary>
        ///     Parses a raw Kubernetes native event into user-friendly events
        /// </summary>
        /// <param name="rawPayload">Raw payload containing the native Kubernetes event</param>
        /// <returns>User-friendly Kubernetes event</returns>
        public virtual IKubernetesEvent ParseFromRawNativeEvent(string rawPayload)
        {
            var parsedPayload = JToken.Parse(rawPayload);

            var sourceComponent = parsedPayload["source"]?["component"]?.ToString()?.ToLower();
            switch (sourceComponent)
            {
                case "cluster-autoscaler":
                    var clusterAutoscalerEventConverter = new ClusterAutoscalerEventConverter();
                    return clusterAutoscalerEventConverter.ConvertFromNativeEvent(parsedPayload);
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }

        private IKubernetesEvent ComposeRawKubernetesEvent(JToken parsedPayload)
        {
            return new RawKubernetesEvent
            {
                Payload = parsedPayload.ToObject<object>()
            };
        }
    }

    public class ClusterAutoscalerEventConverter : EventConverter
    {
        public IKubernetesEvent ConvertFromNativeEvent(JToken parsedPayload)
        {
            var eventReason = parsedPayload["reason"]?.ToString()?.ToLower();
            switch (eventReason)
            {
                case "scaledown":
                    return new ClusterAutoscalerScaleInEvent();
                case "triggeredscaleup":
                    return new ClusterAutoscalerScaleOutEvent
                    {
                        Payload = new ClusterAutoscalerScaleOutEventPayload
                        {
                            Replicas = new ReplicaInfo
                            {
                                Old = 1,
                                New = 2
                            }
                        }
                    };
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }
    }

    public class EventConverter
    {
        protected IKubernetesEvent ComposeRawKubernetesEvent(JToken parsedPayload)
        {
            return new RawKubernetesEvent
            {
                Payload = parsedPayload.ToObject<object>()
            };
        }
    }
}