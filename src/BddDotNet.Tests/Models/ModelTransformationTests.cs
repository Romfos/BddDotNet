using BddDotNet.Gherkin.Models;
using BddDotNet.Tests.Core;
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

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.When("step1", (object?)new string[][] { ["Name", "Value"], ["first", "1"], ["Second", "abcd"], ["third", "3"] });
            });
            services.When(new("step1"), (Model1 model) =>
            {
                traces.Add(model);
            });
        });

        Assert.IsTrue(traces is [Model1 { first: 1, Second: "abcd", third: 3 }]);
    }
}

#pragma warning disable

file class Model1(int first)
{
    public int first = first;
    public decimal? third;
    public string? Second { get; set; }
}
