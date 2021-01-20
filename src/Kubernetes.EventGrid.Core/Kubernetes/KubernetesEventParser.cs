using Kubernetes.EventGrid.Core.Kubernetes.Events;
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
        public IKubernetesEvent ParseFromRawNativeEvent(string rawPayload)
        {
            var parsedPayload = JToken.Parse(rawPayload);

            var eventReason = parsedPayload["reason"]?.ToString();
            switch (eventReason)
            {
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }

        private static IKubernetesEvent ComposeRawKubernetesEvent(JToken parsedPayload)
        {
            return new RawKubernetesEvent
            {
                Payload = parsedPayload.ToObject<object>()
            };
        }
    }
}