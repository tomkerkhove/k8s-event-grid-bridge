using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public class EventConverter
    {
        protected IKubernetesEvent ComposeRawKubernetesEvent(JToken parsedPayload)
        {
            return new KubernetesEvent(KubernetesEventType.Raw, parsedPayload.ToObject<object>());
        }
    }
}
