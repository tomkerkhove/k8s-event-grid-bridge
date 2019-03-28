using System.ComponentModel;
using System.Linq;
using System.Text;
using Kubernetes.EventBridge.Host.Serialization;
using Kubernetes.EventBridge.Host.Configuration.Model;
using Xunit;

namespace Kubernetes.EventBridge.Tests.Unit.Serialization
{
    [Category(category: "Unit")]
    public class RuntimeConfigurationTests
    {
        [Fact]
        public void YamlSerialization_DeserializeCompleteRuntimeConfigration_Succeeds()
        {
            // Arrange
            const string kubernetesNamespace = "sandbox";
            const string eventTopicUri = "https://k8s-event-bridge.westeurope-1.eventgrid.azure.net/api/events";
            const string eventInfoSource = "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-bridge/providers/Microsoft.EventGrid/topics/k8s-event-bridge#k8s-event-bridge";
            const string customHeaderName = "aeg-sas-key";
            const string customHeaderValue = "80Sxc%2FMslQ1gdVbqKtkKRwz0yDoE%2F%2FXGlBg%2Fo5ISgbo%3D";
            const int retryCount = 10;
            var configurationDeserializer = YamlSerialization.CreateDeserializer();
            var rawConfig = GenerateCompleteConfig(kubernetesNamespace, eventTopicUri, customHeaderName, customHeaderValue, retryCount, eventInfoSource);

            // Act
            var runtimeConfiguration = (RuntimeConfiguration)configurationDeserializer.Deserialize(rawConfig, typeof(RuntimeConfiguration));

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Kubernetes);
            Assert.NotNull(runtimeConfiguration.EventInfo);
            Assert.NotNull(runtimeConfiguration.EventTopic);
            Assert.NotNull(runtimeConfiguration.EventTopic.CustomHeaders);
            Assert.NotNull(runtimeConfiguration.EventTopic.Resiliency);
            Assert.NotNull(runtimeConfiguration.EventTopic.Resiliency.Retry);
            Assert.Equal(kubernetesNamespace, runtimeConfiguration.Kubernetes.Namespace);
            Assert.Equal(eventInfoSource, runtimeConfiguration.EventInfo.Source);
            Assert.Equal(eventTopicUri, runtimeConfiguration.EventTopic.Uri);
            Assert.Equal(retryCount, runtimeConfiguration.EventTopic.Resiliency.Retry.Count);
            var customHeader = runtimeConfiguration.EventTopic.CustomHeaders.FirstOrDefault();
            Assert.Equal(customHeaderName, customHeader.Key);
            Assert.Equal(customHeaderValue, customHeader.Value);
        }

        [Fact]
        public void YamlSerialization_DeserializeMinimalRuntimeConfigration_Succeeds()
        {
            // Arrange
            const string kubernetesNamespace = "sandbox";
            const string eventTopicUri = "https://k8s-event-bridge.westeurope-1.eventgrid.azure.net/api/events";
            const string eventInfoSource = "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-bridge/providers/Microsoft.EventGrid/topics/k8s-event-bridge#k8s-event-bridge";
            const int defaultRetryCount = 5;
            var configurationDeserializer = YamlSerialization.CreateDeserializer();
            var rawConfig = GenerateMinimalRawConfig(kubernetesNamespace, eventTopicUri, eventInfoSource);

            // Act
            var runtimeConfiguration = (RuntimeConfiguration)configurationDeserializer.Deserialize(rawConfig, typeof(RuntimeConfiguration));

            // Assert
            Assert.NotNull(runtimeConfiguration);
            Assert.NotNull(runtimeConfiguration.Kubernetes);
            Assert.NotNull(runtimeConfiguration.EventInfo);
            Assert.NotNull(runtimeConfiguration.EventTopic);
            Assert.NotNull(runtimeConfiguration.EventTopic.CustomHeaders);
            Assert.NotNull(runtimeConfiguration.EventTopic.Resiliency);
            Assert.NotNull(runtimeConfiguration.EventTopic.Resiliency.Retry);
            Assert.Equal(kubernetesNamespace, runtimeConfiguration.Kubernetes.Namespace);
            Assert.Equal(eventInfoSource, runtimeConfiguration.EventInfo.Source);
            Assert.Equal(eventTopicUri, runtimeConfiguration.EventTopic.Uri);
            Assert.Equal(defaultRetryCount, runtimeConfiguration.EventTopic.Resiliency.Retry.Count);
            Assert.Empty(runtimeConfiguration.EventTopic.CustomHeaders);
        }

        [Fact]
        public void YamlSerialization_SerializeAndDeserializeValidRuntimeConfigurationWithDefaults_SucceedsWithIdenticalOutput()
        {
            // Arrange
            var kubernetesConfig = BogusData.GenerateKubernetesConfig();
            var eventTopicConfig = BogusData.GenerateEventTopicConfig();
            var eventInfoConfig = BogusData.GenerateEventInfo();
            var runtimeConfiguration = new RuntimeConfiguration()
            {
                Kubernetes = kubernetesConfig,
                EventTopic = eventTopicConfig,
                EventInfo = eventInfoConfig
            };
            var configurationSerializer = YamlSerialization.CreateSerializer();
            var configurationDeserializer = YamlSerialization.CreateDeserializer();

            // Act
            var serializedConfiguration = configurationSerializer.Serialize(runtimeConfiguration);
            var deserializedConfiguration = (RuntimeConfiguration)configurationDeserializer.Deserialize(serializedConfiguration, typeof(RuntimeConfiguration));

            // Assert
            Assert.NotNull(deserializedConfiguration);
            AssertKubernetesConfiguration(deserializedConfiguration.Kubernetes, kubernetesConfig);
            AssertEventInfo(deserializedConfiguration.EventInfo, eventInfoConfig);
            AssertEventTopicConfiguration(deserializedConfiguration.EventTopic, eventTopicConfig);
        }

        private static void AssertEventTopicConfiguration(EventTopicConfiguration eventTopic, EventTopicConfiguration expectedEventTopicConfig)
        {
            Assert.NotNull(eventTopic);
            Assert.Equal(expectedEventTopicConfig.Uri, eventTopic.Uri);
            Assert.NotNull(eventTopic.Resiliency);
            Assert.NotNull(eventTopic.Resiliency.Retry);
            Assert.Equal(expectedEventTopicConfig.Resiliency.Retry.Count, eventTopic.Resiliency.Retry.Count);
            Assert.NotNull(eventTopic.CustomHeaders);
            Assert.Equal(expectedEventTopicConfig.CustomHeaders.Count, eventTopic.CustomHeaders.Count);
            foreach (var customHeader in eventTopic.CustomHeaders)
            {
                Assert.True(expectedEventTopicConfig.CustomHeaders.ContainsKey(customHeader.Key));
                Assert.Equal(expectedEventTopicConfig.CustomHeaders[customHeader.Key], customHeader.Value);
            }
        }

        private static void AssertEventInfo(EventInfo eventInfo, EventInfo expectedEventInfoConfig)
        {
            Assert.NotNull(eventInfo);
            Assert.Equal(expectedEventInfoConfig.Source, eventInfo.Source);
        }

        private static void AssertKubernetesConfiguration(KubernetesConfiguration kubernetesConfiguration, KubernetesConfiguration expectedKubernetesConfig)
        {
            Assert.NotNull(kubernetesConfiguration);
            Assert.Equal(expectedKubernetesConfig.Namespace, kubernetesConfiguration.Namespace);
        }

        private static string GenerateMinimalRawConfig(string kubernetesNamespace, string eventTopicUri, string eventInfoSource)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("kubernetes:");
            stringBuilder.AppendLine($"  namespace: {kubernetesNamespace}");
            stringBuilder.AppendLine("eventTopic:");
            stringBuilder.AppendLine($"  uri: {eventTopicUri}");
            stringBuilder.AppendLine("eventInfo:");
            stringBuilder.Append($"  source: {eventInfoSource}");

            var rawConfig = stringBuilder.ToString();
            return rawConfig;
        }

        private static string GenerateCompleteConfig(string kubernetesNamespace, string eventTopicUri, string customHeaderName, string customHeaderValue, int retryCount, string eventInfoSource)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("kubernetes:");
            stringBuilder.AppendLine($"  namespace: {kubernetesNamespace}");
            stringBuilder.AppendLine("eventTopic:");
            stringBuilder.AppendLine($"  uri: {eventTopicUri}");
            stringBuilder.AppendLine("  customHeaders:");
            stringBuilder.AppendLine($"    {customHeaderName}: {customHeaderValue}");
            stringBuilder.AppendLine($"  resiliency:");
            stringBuilder.AppendLine($"    retry:");
            stringBuilder.AppendLine($"      count: {retryCount}");
            stringBuilder.AppendLine("eventInfo:");
            stringBuilder.Append($"  source: {eventInfoSource}");

            var rawConfig = stringBuilder.ToString();
            return rawConfig;
        }
    }
}