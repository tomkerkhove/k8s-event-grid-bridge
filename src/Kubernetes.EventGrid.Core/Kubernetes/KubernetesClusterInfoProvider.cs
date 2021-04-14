using GuardNet;
using Kubernetes.EventGrid.Core.Kubernetes.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Kubernetes.EventGrid.Core.Kubernetes
{
    public class KubernetesClusterInfoProvider : IKubernetesClusterInfoProvider
    {
        public const string DefaultClusterName = "unknown-cluster";
        private readonly IConfiguration _configuration;

        public KubernetesClusterInfoProvider(IConfiguration configuration)
        {
            Guard.NotNull(configuration, nameof(configuration));

            _configuration = configuration;
        }

        public string GetClusterName()
        {
            var configuredClusterName = _configuration.GetValue<string>("Kubernetes_Cluster_Name");
            
            if (string.IsNullOrWhiteSpace(configuredClusterName))
            {
                return DefaultClusterName;
            } 

            return configuredClusterName;
        }
    }
}