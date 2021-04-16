using System;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public abstract class EventConverter
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

        protected KubernetesEvent CreateKubernetesEvent(KubernetesEventType eventType, string source, string subject, object payload, JToken parsedPayload)
        {
            var @namespace = GetKubernetesNamespaceThatEmittedEvent(parsedPayload);

            return new KubernetesEvent(eventType, payload, @namespace)
            {
                Source = new Uri(source),
                Subject = subject
            };
        }
    }
}
