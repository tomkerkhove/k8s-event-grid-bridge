using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;

namespace Kubernetes.EventGrid.Core.Kubernetes.Interfaces
{
    public interface IKubernetesEventParser
    {
        /// <summary>
        ///     Parses a raw Kubernetes native event into user-friendly events
        /// </summary>
        /// <param name="rawPayload">Raw payload containing the native Kubernetes event</param>
        /// <returns>User-friendly Kubernetes event</returns>
        public IKubernetesEvent ParseFromRawNativeEvent(string rawPayload);
    }
}
