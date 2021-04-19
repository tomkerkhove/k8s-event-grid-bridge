using System.Collections.Generic;

namespace Kubernetes.EventGrid.Bridge.Contracts.Events.Deployments
{
    public class DeploymentScaleEventPayload
    {
        public DeploymentInfo Deployment { get; set; }
        public ReplicaSetInfo ReplicaSet { get; set; }
        public ReplicaInfo Replicas { get; set; }
    }

    public class DeploymentInfo
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public Dictionary<string, string> Labels { get; set; } = new Dictionary<string, string>();
    }

    public class ReplicaSetInfo
    {
        public string Name { get; set; }
    }
}
