using System;
using System.Text;
using Kubernetes.EventBridge.Host.Configuration.Model;
using Kubernetes.EventBridge.Host.Serialization;

namespace Kubernetes.EventBridge.Host.Configuration
{
    public interface IRuntimeConfigurationProvider
    {
        RuntimeConfiguration Get();
    }

    public class RuntimeConfigurationProvider : IRuntimeConfigurationProvider
    {
        public RuntimeConfiguration Get()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("kubernetes:");
            stringBuilder.AppendLine("  namespace: sandbox");
            stringBuilder.AppendLine("eventTopic:");
            stringBuilder.AppendLine("  uri: \"https://k8s-event-bridge.westeurope-1.eventgrid.azure.net/api/events\"");
            stringBuilder.AppendLine("  customHeaders:");
            stringBuilder.AppendLine("    aeg-sas-key: 80Sxc%2FMslQ1gdVbqKtkKRwz0yDoE%2F%2FXGlBg%2Fo5ISgbo%3D");
            stringBuilder.AppendLine($"    request-id: {Guid.NewGuid()}");
            stringBuilder.AppendLine("  resiliency:");
            stringBuilder.AppendLine("    retry:");
            stringBuilder.AppendLine("      count: 10");
            stringBuilder.AppendLine("eventInfo:");
            stringBuilder.Append("  source: /subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-bridge/providers/Microsoft.EventGrid/topics/k8s-event-bridge#k8s-event-bridge");

            var rawConfig = stringBuilder.ToString();

            var yamlDeserializer = YamlSerialization.CreateDeserializer();
            var deserializedRuntimeConfig = (RuntimeConfiguration)yamlDeserializer.Deserialize(rawConfig, typeof(RuntimeConfiguration));

            return deserializedRuntimeConfig;
        }
    }
}