using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Core.Kubernetes.Parsers;
using Kubernetes.EventGrid.Tests.Unit.Events;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes.Deployments
{
    public class DeploymentControllerEventParserUnitTests : DeploymentControllerEventParsingTests
    {
        [Fact]
        public void ParseForScalingReplicaSet_RawScaleOutEventMessage_Succeeds()
        {
            // Arrange
            var rawClusterAutoscalerScaleDownEvent = KubernetesEventSamples.GetRawReplicaSetScaleOutEvent();
            var rawJToken = JToken.Parse(rawClusterAutoscalerScaleDownEvent);
            const HorizontalScaleDirection expectedScaleDirection = HorizontalScaleDirection.Out;

            // Act
            var parseOutcome = DeploymentControllerEventParser.ParseForScalingReplicaSet(rawJToken);

            // Assert
            AssertForExpectedDeploymentScalingPayload(expectedScaleDirection, parseOutcome.ScaleDirection, parseOutcome.Payload);
        }

        [Fact]
        public void ParseForScalingReplicaSet_RawScaleInEventMessage_Succeeds()
        {
            // Arrange
            var rawClusterAutoscalerScaleDownEvent = KubernetesEventSamples.GetRawReplicaSetScaleInEvent();
            var rawJToken = JToken.Parse(rawClusterAutoscalerScaleDownEvent);
            const HorizontalScaleDirection expectedScaleDirection = HorizontalScaleDirection.In;

            // Act
            var parseOutcome = DeploymentControllerEventParser.ParseForScalingReplicaSet(rawJToken);

            // Assert
            AssertForExpectedDeploymentScalingPayload(expectedScaleDirection, parseOutcome.ScaleDirection, parseOutcome.Payload);
        }
    }
}
