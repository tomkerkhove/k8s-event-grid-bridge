using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Extensions;
using Kubernetes.EventGrid.Tests.Unit.Extensions.Enums;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit.Extensions
{
    public class EnumExtensionsUnitTests : UnitTest
    {
        [Fact]
        public void GetDescription_ValueHasDescription_DescriptionIsReturned()
        {
            // Arrange
            const string expectedValue = "Kubernetes.Events.Raw";
            var enumValue = KubernetesEventType.Raw;

            // Act
            var enumDescription = enumValue.GetDescription();

            // Assert
            Assert.Equal(expectedValue, enumDescription);
        }

        [Fact]
        public void GetDescription_ValueHasNoDescriptionAndNoDefaultSpecified_EnumValueIsReturned()
        {
            // Arrange
            const string expectedValue = "OptionTwo";
            var enumValue = ExampleEnum.OptionTwo;

            // Act
            var enumDescription = enumValue.GetDescription();

            // Assert
            Assert.Equal(expectedValue, enumDescription);
        }

        [Fact]
        public void GetDescription_ValueHasNoDescriptionAndHasDefaultSpecified_DefaultIsReturned()
        {
            // Arrange
            const string expectedValue = "DefaultOption";
            var enumValue = ExampleEnum.OptionTwo;

            // Act
            var enumDescription = enumValue.GetDescription(defaultDescription: expectedValue);

            // Assert
            Assert.Equal(expectedValue, enumDescription);
        }
    }
}
