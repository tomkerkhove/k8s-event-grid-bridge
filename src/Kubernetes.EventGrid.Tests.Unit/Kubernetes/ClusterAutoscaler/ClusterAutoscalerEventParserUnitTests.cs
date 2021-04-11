using Bogus;
using Kubernetes.EventGrid.Core.Kubernetes.Converters;
using Kubernetes.EventGrid.Core.Kubernetes.Parsers;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes.ClusterAutoscaler
{
    public class ClusterAutoscalerEventParserUnitTests : UnitTest
    {
        [Fact]
        public void ParseForClusterScalingOut_RawEventMessage_Succeeds()
        {
            // Arrange
            var expectedNodeGroupName = BogusGenerator.Name.FullName().Replace(" ", "-");
            var oldNodeCount = BogusGenerator.Random.Int(100, 200);
            var newNodeCount = BogusGenerator.Random.Int(200, 300);
            var maximumNodeCount = BogusGenerator.Random.Int(300, 400);
            var eventMessage = $"pod triggered scale-up: [{{{expectedNodeGroupName} {oldNodeCount}->{newNodeCount} (max: {maximumNodeCount})}}]";

            // Act
            var nodeGroupResizeInformation = ClusterAutoscalerEventParser.ParseForClusterScalingOut(eventMessage);

            // Assert
            Assert.NotNull(nodeGroupResizeInformation);
            Assert.NotNull(nodeGroupResizeInformation.SizeInfo);
            Assert.Equal(expectedNodeGroupName, nodeGroupResizeInformation.Name);
            Assert.Equal(oldNodeCount, nodeGroupResizeInformation.SizeInfo.Old);
            Assert.Equal(newNodeCount, nodeGroupResizeInformation.SizeInfo.New);
            Assert.Equal(maximumNodeCount, nodeGroupResizeInformation.SizeInfo.Maximum);
        }
    }
}
