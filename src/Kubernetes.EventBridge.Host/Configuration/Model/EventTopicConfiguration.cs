using System.Collections.Generic;

namespace Kubernetes.EventBridge.Host.Configuration.Model
{
    public class EventTopicConfiguration
    {
        public string Uri { get; set; } = null;
        public Dictionary<string, string> CustomHeaders { get; set; } = new Dictionary<string, string>();
        public ResiliencyConfiguration Resiliency { get; set; } = new ResiliencyConfiguration();
    }
}