using k8s.Models;

namespace Kubernetes.EventBridge.Sample
{
    interface IEventHandler
    {
        void HandleEvent(V1Event obj);
    }
}