using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests;

[TestClass]
public sealed class GherkinTests
{
    [TestMethod]
    public async Task FeatureGeneratorAndStepDiscoveryTest()
    {
        var traces = new List<int>();

        await Platform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.SourceGeneratedGherkinScenarios();
            services.SourceGeneratedGherkinSteps();
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }
}
