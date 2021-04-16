using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.Deployments;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventGrid.Core.Kubernetes.Parsers
{
    public class DeploymentControllerEventParser
    {
        /// <summary>
        /// Regex to parse the message to get scaling information 
        /// </summary>
        /// <example>Scaled up replica set k8s-event-grid-bridge-workload-76888d9cc9 to 1</example>
        private const string MessageRegex = @"Scaled ([^\s]+) replica set ([^\s]+) to ([0-9]*)";

        public static (HorizontalScaleDirection ScaleDirection, DeploymentScaleEventPayload Payload) ParseForScalingReplicaSet(JToken @event)
        {
            var deploymentInfoToken = @event["involvedObject"];
            if (deploymentInfoToken == null || deploymentInfoToken["kind"]?.ToString()?.Equals("Deployment", StringComparison.InvariantCultureIgnoreCase) == false)
            {
                throw new Exception("No deployment info was found in 'involvedObject'");
            }
            
            // Interpret message to get more detailed information
            var eventMessage = @event["message"]?.ToString();
            if (Regex.IsMatch(eventMessage, MessageRegex) == false)
            {
                throw new ArgumentException("Provided input does not meet the needs to determine resized cluster info");
            }

            var matchingEntries = Regex.Match(eventMessage, MessageRegex);

            var payload = ComposePayload(deploymentInfoToken, matchingEntries);
            var scaleDirection = DetermineScaleDirection(matchingEntries);

            return (scaleDirection, payload);
        }

        private static DeploymentScaleEventPayload ComposePayload(JToken deploymentInfoToken, Match matchingEntries)
        {
            var payload = new DeploymentScaleEventPayload
            {
                Deployment = new DeploymentInfo
                {
                    Name = deploymentInfoToken["name"]?.ToString(),
                    Namespace = deploymentInfoToken["namespace"]?.ToString(),
                },
                ReplicaSet = new ReplicaSetInfo
                {
                    Name = matchingEntries.Groups[2].Value
                },
                Replicas = new ReplicaInfo
                {
                    New = int.Parse(matchingEntries.Groups[3].Value)
                }
            };

            var deploymentLabelToken = deploymentInfoToken["labels"];
            if (deploymentLabelToken != null)
            {
                payload.Deployment.Labels = deploymentLabelToken.ToObject<Dictionary<string, string>>();
            }

            return payload;
        }

        private static HorizontalScaleDirection DetermineScaleDirection(Match regexMatchingEntries)
        {
            // Interpret regex results
            var rawScaleDirection = regexMatchingEntries.Groups[1].Value;

            HorizontalScaleDirection scaleDirection = HorizontalScaleDirection.NotSpecified;

            // Converting the raw scale direction from Kubernetes to our enum
            // See GitHub for more info: https://github.com/kubernetes/kubernetes/blob/master/pkg/controller/deployment/sync.go#L342-L351
            if (rawScaleDirection.Equals("up", StringComparison.InvariantCultureIgnoreCase))
            {
                scaleDirection = HorizontalScaleDirection.Out;
            }
            else if (rawScaleDirection.Equals("down", StringComparison.InvariantCultureIgnoreCase))
            {
                scaleDirection = HorizontalScaleDirection.In;
            }

            return scaleDirection;
        }
    }
}
