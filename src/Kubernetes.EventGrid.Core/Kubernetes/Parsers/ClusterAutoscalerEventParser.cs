using System;
using System.Text.RegularExpressions;
using Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler;

namespace Kubernetes.EventGrid.Core.Kubernetes.Parsers
{
    public static class ClusterAutoscalerEventParser
    {
        /// <summary>
        /// Regex to parse the cluster resize information
        /// </summary>
        /// <example>pod triggered scale-up: [{aks-agentpool-11593772-vmss 1->2 (max: 2)}]</example>
        private const string ClusterResizeRegex = @"\[\{([^\s]+) ([0-9]*)->([0-9]*) \(max: ([0-9]*)";

        public static NodeGroupResizeInformation ParseForClusterScalingOut(string eventMessage)
        {
            if (Regex.IsMatch(eventMessage, ClusterResizeRegex) == false)
            {
                throw new ArgumentException("Provided input does not meet the needs to determine resized cluster info");
            }

            var matchingEntries = Regex.Match(eventMessage, ClusterResizeRegex);

            var nodeGroupName = matchingEntries.Groups[1].Value;
            var newSizeInfo = new NewNodeGroupSizeInfo
            {
                New = int.Parse(matchingEntries.Groups[3].Value),
                Old = int.Parse(matchingEntries.Groups[2].Value),
                Maximum = int.Parse(matchingEntries.Groups[4].Value)
            };

            var nodeGroupResizeInfo = new NodeGroupResizeInformation
            {
                Name = nodeGroupName,
                SizeInfo = newSizeInfo
            };

            return nodeGroupResizeInfo;
        }
    }
}
