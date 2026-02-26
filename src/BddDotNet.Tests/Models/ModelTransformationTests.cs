using BddDotNet.Gherkin.Models;
using BddDotNet.Scenarios;
using BddDotNet.Steps;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Models;

[TestClass]
public sealed class ModelTransformationTests
{
    [TestMethod]
    public async Task ModelTransformationTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.ModelTransformation<Model1>();

            services.When(new("step1"), (Model1 model) =>
            {
                traces.Add(model);
            });

            services.Scenario("feature1", "scenario1", async scenario =>
            {
                await scenario.When("step1", (object?)new string[][] { ["Name", "Value"], ["first", "1"], ["Second", "abcd"], ["third", "3"] });
            });
        });

        Assert.IsTrue(traces is [Model1 { first: 1, Second: "abcd", third: 3 }]);
    }

    [TestMethod]
    public async Task InvalidDataTableFormatTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.ModelTransformation<Model1>();
            services.AfterScenario<AfterScenario1>();

            services.When(new("step1"), (Model1 model) =>
            {
                traces.Add(model);
            });

            services.Scenario("feature1", "scenario1", async scenario =>
            {
                await scenario.When("step1", (object?)new string[][] { ["Name", "Value1"], ["first", "1"], ["Second", "abcd"], ["third", "3"] });
            });
        });

        Assert.IsTrue(traces is ["Invalid table format. Name-Value table is expected"]);
    }
}

#pragma warning disable

file class Model1(int first)
{
    public int first = first;
    public decimal? third;
    public string? Second { get; set; }
}

file sealed class AfterScenario1(List<object?> traces) : IAfterScenario
{
    public Task AfterScenarioAsync(ScenarioContext context, Exception? exception)
    {
        traces.Add(exception?.Message);
        return Task.CompletedTask;
    }
}
