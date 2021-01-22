using CloudNative.CloudEvents;

namespace Kubernetes.EventGrid.Core.CloudEvents.Interfaces
{
    public interface ICloudEventFactory
    {
        /// <summary>
        ///     Creates a CloudEvent for a raw Kubernetes event
        /// </summary>
        /// <param name="payload">Raw Kubernetes event</param>
        CloudEvent CreateFromRawKubernetesEvent(string payload);
    }
}
