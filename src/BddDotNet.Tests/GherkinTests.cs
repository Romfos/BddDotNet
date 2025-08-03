using BddDotNet.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests;

[TestClass]
public sealed class GherkinTests
{
    [TestMethod]
    public async Task GherkinFeatureAndStepsTest()
    {
        var traces = new Dictionary<string, List<object>>();

        await Platform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.AddScoped<TraceService>();

            services.SourceGeneratedGherkinScenarios();
            services.SourceGeneratedGherkinSteps();
        });

        Assert.IsTrue(traces["simple steps"] is
            [
                "this is simple given step",
                "this is simple when step",
                "this is simple then step"
            ]);

        Assert.IsTrue(traces["simple async steps"] is
            [
                "this is async task given step",
                "this is async value task given step",
            ]);

        Assert.IsTrue(traces["steps with arguments"] is
            [
                "abcd",
                string[][] and [["book", "price"], ["sharpener", "30"], ["pencil", "15"]]
            ]);

        Assert.IsTrue(traces["And keyword steps"] is
            [
                "this is simple given step",
                "this is simple when step",
                "this is simple when step"
            ]);
    }
}
