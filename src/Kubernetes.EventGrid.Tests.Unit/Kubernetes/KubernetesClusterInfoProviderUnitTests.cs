using System.Collections.Generic;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.ClusterAutoscaler;
using Kubernetes.EventGrid.Core.Kubernetes;
using Kubernetes.EventGrid.Tests.Unit.Events;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Kubernetes
{
    public class KubernetesClusterInfoProviderUnitTests : UnitTest
    {
        [Fact]
        public void GetClusterName_NameIsConfigured_ReturnsConfiguredName()
        {
            // Arrange
            var expectedClusterName = BogusGenerator.Name.FullName();
            var inMemoryConfiguration = new Dictionary<string, string>
            {
                {"Kubernetes_Cluster_Name", expectedClusterName}
            };
            var config = CreateInMemoryConfiguration(inMemoryConfiguration);
            var kubernetesClusterInfoProvider = new KubernetesClusterInfoProvider(config);

            // Act
            var clusterName = kubernetesClusterInfoProvider.GetClusterName();

            // Assert
            Assert.Equal(expectedClusterName, clusterName);
        }

        [Fact]
        public void GetClusterName_NameIsNotConfigured_ReturnsDefaultName()
        {
            // Arrange
            var inMemoryConfiguration = new Dictionary<string, string>();
            var config = CreateInMemoryConfiguration(inMemoryConfiguration);
            var kubernetesClusterInfoProvider = new KubernetesClusterInfoProvider(config);

            // Act
            var clusterName = kubernetesClusterInfoProvider.GetClusterName();

            // Assert
            Assert.Equal(KubernetesClusterInfoProvider.DefaultClusterName, clusterName);
        }

        private static IConfigurationRoot CreateInMemoryConfiguration(Dictionary<string, string> inMemoryConfiguration)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();
            return config;
        }
    }
}
