namespace Kubernetes.EventBridge.Host.Configuration.Model
{
    public class ResiliencyConfiguration
    {
        public RetryConfiguration Retry { get; set; }=new RetryConfiguration();
    }
}