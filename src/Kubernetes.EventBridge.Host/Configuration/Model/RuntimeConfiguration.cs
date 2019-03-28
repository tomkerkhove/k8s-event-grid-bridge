namespace Kubernetes.EventBridge.Host.Configuration.Model
{
    public class RuntimeConfiguration
    {
        public KubernetesConfiguration Kubernetes { get; set; }
        public EventTopicConfiguration EventTopic { get; set; }
        public EventInfo EventInfo { get; set; }
    }
}