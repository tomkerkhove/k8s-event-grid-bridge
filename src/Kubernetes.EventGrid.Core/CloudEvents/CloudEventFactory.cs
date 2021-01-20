using System;
using System.Net.Mime;
using CloudNative.CloudEvents;
using Kubernetes.EventGrid.Core.CloudEvents.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes.Interfaces;

namespace Kubernetes.EventGrid.Core.CloudEvents
{
    public class CloudEventFactory : ICloudEventFactory
    {
        private readonly IKubernetesEventParser _kubernetesEventParser;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="kubernetesEventParser">Converter to convert raw Kubernetes events into user-friendly events</param>
        public CloudEventFactory(IKubernetesEventParser kubernetesEventParser)
        {
            _kubernetesEventParser = kubernetesEventParser;
        }

        /// <summary>
        ///     Creates a CloudEvent for a raw Kubernetes event
        /// </summary>
        /// <param name="payload">Raw Kubernetes event</param>
        public CloudEvent CreateFromRawKubernetesEvent(string payload)
        {
            var kubernetesEvent = _kubernetesEventParser.ParseFromRawNativeEvent(payload);
            var eventType = kubernetesEvent.Type.GetDescription();
            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V1_0, eventType, new Uri("http://kubernetes"))
            {
                DataContentType = new ContentType("application/json"),
                Data = kubernetesEvent.Payload
            };

            return cloudEvent;
        }
    }
}