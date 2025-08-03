using BddDotNet.Extensibility;

namespace BddDotNet.Services;

#nullable disable

internal sealed class TestContext : ITestContext
{
    public string Feature { get; set; }
    public string Scenario { get; set; }
}
