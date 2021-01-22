using Kubernetes.EventGrid.Core.Kubernetes;
using Kubernetes.EventGrid.Core.Kubernetes.Enums;
using Kubernetes.EventGrid.Tests.Unit.Events;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes
{
    [Trait("Category", "Unit")]
    public class KubernetesEventParserUnitTests
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
    }
}
