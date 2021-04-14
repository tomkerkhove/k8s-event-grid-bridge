using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public class EventConverter
    {
        protected string GetKubernetesNamespaceThatEmittedEvent(JToken parsedPayload)
        {
            return parsedPayload["metadata"]?["namespace"]?.ToString();
        }

        protected IKubernetesEvent ComposeRawKubernetesEvent(JToken parsedPayload)
        {
            var @namespace = GetKubernetesNamespaceThatEmittedEvent(parsedPayload);
            return new KubernetesEvent(KubernetesEventType.Raw, parsedPayload.ToObject<object>(), @namespace);
        }
    }
}
