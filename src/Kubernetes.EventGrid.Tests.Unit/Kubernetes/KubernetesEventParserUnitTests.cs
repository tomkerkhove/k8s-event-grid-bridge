using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler;
using Kubernetes.EventGrid.Core.Kubernetes;
using Kubernetes.EventGrid.Tests.Unit.Events;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes
{
    public class KubernetesEventParserUnitTests : UnitTest
    {
        [Fact]
        public void ParseFromRawNativeEvent_ParseRawEvent_ReturnsRawEvent()
        {
            // Arrange
            var kubernetesEventParser = new KubernetesEventParser();
            var rawKubernetesEvent = KubernetesEventSamples.GetRawContainerStartedEvent();

            // Act
            var kubernetesEvent = kubernetesEventParser.ParseFromRawNativeEvent(rawKubernetesEvent);

            // Assert
            Assert.NotNull(kubernetesEvent);
            Assert.Equal(KubernetesEventType.Raw, kubernetesEvent.Type);
            Assert.NotNull(kubernetesEvent.Payload);
        }

        [Fact]
        public void ParseFromRawNativeEvent_ParseRawClusterAutoscalerScaleInEvent_ReturnsClusterAutoscalerScaleInEvent()
        {
            // Arrange
            var kubernetesEventParser = new KubernetesEventParser();
            var rawClusterAutoscalerScaleDownEvent = KubernetesEventSamples.GetRawClusterAutoscalerScaleOutEvent();

            // Act
            var kubernetesEvent = kubernetesEventParser.ParseFromRawNativeEvent(rawClusterAutoscalerScaleDownEvent);

            // Assert
            Assert.NotNull(kubernetesEvent);
            Assert.Equal(KubernetesEventType.ClusterAutoscalerScaleOut, kubernetesEvent.Type);
            Assert.NotNull(kubernetesEvent.Payload);
            var clusterAutoscalerScaleEventPayload = kubernetesEvent.Payload as ClusterAutoscalerScaleEventPayload;
            Assert.NotNull(clusterAutoscalerScaleEventPayload);
            Assert.NotNull(clusterAutoscalerScaleEventPayload.NodeGroup);
            Assert.Equal("aks-agentpool-11593772-vmss", clusterAutoscalerScaleEventPayload.NodeGroup.Name);
            Assert.NotNull(clusterAutoscalerScaleEventPayload.NodeGroup.SizeInfo);
            Assert.Equal(1, clusterAutoscalerScaleEventPayload.NodeGroup.SizeInfo.Old);
            Assert.Equal(2, clusterAutoscalerScaleEventPayload.NodeGroup.SizeInfo.New);
            Assert.Equal(2, clusterAutoscalerScaleEventPayload.NodeGroup.SizeInfo.Maximum);
        }
    }
}
