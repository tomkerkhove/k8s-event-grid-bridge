using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public interface IEventConverter
    {
        IKubernetesEvent ConvertFromNativeEvent(JToken parsedPayload);
    }
}