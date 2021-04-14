using CloudNative.CloudEvents;
using Kubernetes.EventGrid.Core.Kubernetes;

namespace Kubernetes.EventGrid.Core.CloudEvents.Interfaces
{
    public interface ICloudEventFactory
    {
        /// <summary>
        ///     Creates a CloudEvent for a raw Kubernetes event
        /// </summary>
        /// <param name="payload">Raw Kubernetes event</param>
        (CloudEvent Event, KubernetesEventContext KubernetesEventContext) CreateFromRawKubernetesEvent(string payload);
    }
}
