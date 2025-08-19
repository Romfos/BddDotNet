using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Gherkin;

[TestClass]
public sealed class GherkinSourceGeneratorTests
{
    [TestMethod]
    public async Task GherkinFeatureAndStepsTest()
    {
        var traces = new Dictionary<string, List<object>>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.AddScoped<GherkinTraceService>();

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
                string[][] and [["book", "price"], ["sharpener", "30"], ["pencil", "15"]],
                """
                this is DocString text
                with multiple lines
                """
            ]);

        Assert.IsTrue(traces["And keyword steps"] is
            [
                "this is simple given step",
                "this is simple given step",
                "this is simple when step",
                "this is simple when step"
            ]);

        Assert.IsTrue(traces["#1. scenario outline with 2 examples"] is
            [
                "sharpener",
                string[][] and [["book", "price"], ["sharpener", "30"], ["static", "99"]]
            ]);

        Assert.IsTrue(traces["#2. scenario outline with 2 examples"] is
            [
                "pencil",
                string[][] and [["book", "price"], ["pencil", "15"], ["static", "99"]]
            ]);

        Assert.IsTrue(traces["scenario from rule"] is
            [
                "this is simple given step",
            ]);

        Assert.IsTrue(traces["#1. scenario outline from rule"] is
            [
                "sharpener"
            ]);

        Assert.IsTrue(traces["#2. scenario outline from rule"] is
            [
                "pencil"
            ]);

        Assert.IsTrue(traces["simple steps with background"] is
            [
                "this is simple given step",
                "this is simple when step",
            ]);

        Assert.IsTrue(traces["scenario from rule with background"] is
            [
                "this is simple when step",
                "this is simple then step",
            ]);
    }
}
