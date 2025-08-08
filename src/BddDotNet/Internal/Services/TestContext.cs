using BddDotNet.Extensibility;

namespace BddDotNet.Internal.Services;

internal sealed class TestContext : ITestContext
{
    public required string Feature { get; set; }
    public required string Scenario { get; set; }
}
