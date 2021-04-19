using System;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes.Parsers;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Converters
{
    public class DeploymentControllerEventConverter : EventConverter, IEventConverter
    {
        private const string EventSource = "http://kubernetes/core/controllers/deployment";

        public IKubernetesEvent ConvertFromNativeEvent(JToken parsedPayload)
        {
            var eventReason = parsedPayload["reason"]?.ToString()?.ToLower();
            switch (eventReason)
            {
                case "scalingreplicaset":
                    return ConvertScalingReplicaSet(parsedPayload);
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }

        private IKubernetesEvent ConvertScalingReplicaSet(JToken parsedPayload)
        {
            var parsingResult = DeploymentControllerEventParser.ParseForScalingReplicaSet(parsedPayload);

            KubernetesEventType eventType;
            switch (parsingResult.ScaleDirection)
            {
                case HorizontalScaleDirection.In:
                    eventType = KubernetesEventType.DeploymentScaleIn;
                    break;
                case HorizontalScaleDirection.Out:
                    eventType = KubernetesEventType.DeploymentScaleOut;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parsingResult.ScaleDirection), parsingResult.ScaleDirection, "Unable to determine event type for scale direction");
            }

            return CreateKubernetesEvent(eventType, EventSource, $"/namespaces/{parsingResult.Payload.Deployment.Namespace}/deployments/{parsingResult.Payload.Deployment.Name}", parsingResult.Payload, parsedPayload);
        }
    }
}
