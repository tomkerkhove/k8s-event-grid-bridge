using Bogus;
using Xunit;

namespace Kubernetes.EventGrid.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class UnitTest
    {
        protected Faker BogusGenerator { get; } = new Faker();
    }
}
