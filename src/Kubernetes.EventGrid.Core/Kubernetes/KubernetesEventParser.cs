﻿using Kubernetes.EventGrid.Core.Kubernetes.Converters;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Kubernetes.EventGrid.Core.Kubernetes.Interfaces;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes
{
    public class KubernetesEventParser : EventConverter, IKubernetesEventParser
    {
        /// <summary>
        ///     Parses a raw Kubernetes native event into user-friendly events
        /// </summary>
        /// <param name="rawPayload">Raw payload containing the native Kubernetes event</param>
        /// <returns>User-friendly Kubernetes event</returns>
        public virtual IKubernetesEvent ParseFromRawNativeEvent(string rawPayload)
        {
            var parsedPayload = JToken.Parse(rawPayload);

            var sourceComponent = parsedPayload["source"]?["component"]?.ToString()?.ToLower();
            switch (sourceComponent)
            {
                case "deployment-controller":
                    var deploymentController = new DeploymentControllerEventConverter();
                    return deploymentController.ConvertFromNativeEvent(parsedPayload);
                case "cluster-autoscaler":
                    var clusterAutoscalerEventConverter = new ClusterAutoscalerEventConverter();
                    return clusterAutoscalerEventConverter.ConvertFromNativeEvent(parsedPayload);
                default:
                    return ComposeRawKubernetesEvent(parsedPayload);
            }
        }
    }
}