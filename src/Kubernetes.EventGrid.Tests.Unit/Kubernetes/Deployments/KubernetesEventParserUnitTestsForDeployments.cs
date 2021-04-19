using System.Linq;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler;
using Kubernetes.EventGrid.Bridge.Contracts.Events.Deployments;
using Kubernetes.EventGrid.Core.Kubernetes;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Kubernetes.EventGrid.Tests.Unit.Events;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes.Deployments
{
    public class KubernetesEventParserUnitTestsForDeployments : DeploymentControllerEventParsingTests
    {
        [Fact]
        public void ParseFromRawNativeEvent_ParseRawReplicaSetScaleInEvent_ReturnsDeploymentScaleOutEvent()
        {
            // Arrange
            var kubernetesEventParser = new KubernetesEventParser();
            var rawClusterAutoscalerScaleDownEvent = KubernetesEventSamples.GetRawReplicaSetScaleOutEvent();

            // Act
            var kubernetesEvent = kubernetesEventParser.ParseFromRawNativeEvent(rawClusterAutoscalerScaleDownEvent);

            // Assert
            AssertForExpectedDeploymentScalingEvent(KubernetesEventType.DeploymentScaleOut, kubernetesEvent);
        }

        [Fact]
        public void ParseFromRawNativeEvent_ParseRawReplicaSetScaleInEvent_ReturnsDeploymentScaleInEvent()
        {
            // Arrange
            var kubernetesEventParser = new KubernetesEventParser();
            var rawClusterAutoscalerScaleDownEvent = KubernetesEventSamples.GetRawReplicaSetScaleInEvent();

            // Act
            var kubernetesEvent = kubernetesEventParser.ParseFromRawNativeEvent(rawClusterAutoscalerScaleDownEvent);

            // Assert
            AssertForExpectedDeploymentScalingEvent(KubernetesEventType.DeploymentScaleIn, kubernetesEvent);
        }
    }
}
