using System.Collections.Generic;
using System.Linq;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.Deployments;
using Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes.Deployments
{
    public class DeploymentControllerEventParsingTests : UnitTest
    {
        // Replicas
        protected const int ExpectedNewReplicaCountForScaleOut = 20;
        protected const int ExpectedNewReplicaCountForScaleIn = 10;

        // Deployment
        protected const string ExpectedDeploymentName = "k8s-event-grid-bridge-workload";
        protected const string ExpectedDeploymentNamespace = "monitoring";
        protected Dictionary<string, string> ExpectedDeploymentLabels = new Dictionary<string, string>
        {
            {"app", "k8s-event-grid-bridge"}
        };

        // Replica Set
        protected const string ExpectedReplicaSetName = "k8s-event-grid-bridge-workload-76888d9cc9";

        protected void AssertForExpectedDeploymentScalingEvent(KubernetesEventType expectedKubernetesEventType, IKubernetesEvent kubernetesEvent)
        {
            int expectedReplicaCount = expectedKubernetesEventType == KubernetesEventType.DeploymentScaleIn ? ExpectedNewReplicaCountForScaleIn : ExpectedNewReplicaCountForScaleOut;

            Assert.NotNull(kubernetesEvent);
            Assert.Equal(expectedKubernetesEventType, kubernetesEvent.Type);
            Assert.NotNull(kubernetesEvent.Payload);
            var deploymentScaleEventPayload = kubernetesEvent.Payload as DeploymentScaleEventPayload;
            AssertPayload(deploymentScaleEventPayload, expectedReplicaCount);
        }

        protected void AssertForExpectedDeploymentScalingPayload(HorizontalScaleDirection expectedScaleDirection, HorizontalScaleDirection scaleDirection, DeploymentScaleEventPayload deploymentScaleEventPayload)
        {
            int expectedReplicaCount = expectedScaleDirection == HorizontalScaleDirection.In ? ExpectedNewReplicaCountForScaleIn : ExpectedNewReplicaCountForScaleOut;

            Assert.Equal(expectedScaleDirection, scaleDirection);
            AssertPayload(deploymentScaleEventPayload, expectedReplicaCount);
        }

        private void AssertPayload(DeploymentScaleEventPayload deploymentScaleEventPayload, int expectedReplicaCount)
        {
            Assert.NotNull(deploymentScaleEventPayload);
            Assert.NotNull(deploymentScaleEventPayload.Deployment);
            Assert.NotNull(deploymentScaleEventPayload.Deployment.Labels);
            Assert.Equal(ExpectedDeploymentName, deploymentScaleEventPayload.Deployment.Name);
            Assert.Equal(ExpectedDeploymentNamespace, deploymentScaleEventPayload.Deployment.Namespace);
            Assert.Single(deploymentScaleEventPayload.Deployment.Labels);
            var deploymentLabel = deploymentScaleEventPayload.Deployment.Labels.First();
            var expectedDeploymentLabel = ExpectedDeploymentLabels.First();
            Assert.Equal(expectedDeploymentLabel.Key, deploymentLabel.Key);
            Assert.Equal(expectedDeploymentLabel.Value, deploymentLabel.Value);
            Assert.NotNull(deploymentScaleEventPayload.ReplicaSet);
            Assert.Equal(ExpectedReplicaSetName, deploymentScaleEventPayload.ReplicaSet.Name);
            Assert.NotNull(deploymentScaleEventPayload.Replicas);
            Assert.Equal(expectedReplicaCount, deploymentScaleEventPayload.Replicas.New);
        }
    }
}
