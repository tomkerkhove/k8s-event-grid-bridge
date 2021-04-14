namespace Kubernetes.EventGrid.Core.Kubernetes.Interfaces
{
    public interface IKubernetesClusterInfoProvider
    {
        string GetClusterName();
    }
}