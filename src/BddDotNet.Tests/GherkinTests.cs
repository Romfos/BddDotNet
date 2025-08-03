using BddDotNet.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests;

[TestClass]
public sealed class GherkinTests
{
    [TestMethod]
    public async Task FeatureGeneratorAndStepDiscoveryTest()
    {
        var traceService = new TraceService();

        await Platform.RunTestAsync(services =>
        {
            services.AddSingleton(traceService);

            services.SourceGeneratedGherkinScenarios();
            services.SourceGeneratedGherkinSteps();
        });

        Assert.IsTrue(traceService is
        {
            Step1: true,
            Step2: true,
            Step3: true,
            Step4: "abcd",
            Step5: [["book", "price"], ["sharpener", "30"], ["pencil", "15"]]
        });
    }
}
