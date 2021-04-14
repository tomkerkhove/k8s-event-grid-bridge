using GuardNet;

namespace Kubernetes.EventGrid.Core.Kubernetes
{
    public class KubernetesEventContext
    {
        public string Namespace { get; }

        public KubernetesEventContext(string @namespace)
        {
            Guard.NotNullOrWhitespace(@namespace, nameof(@namespace));

            Namespace = @namespace;
        }
    }
}
