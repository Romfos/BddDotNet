using BddDotNet.Gherkin.CSharpExpressions;
using BddDotNet.Tests.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.CSharpExpressions;

[TestClass]
public sealed class CSharpExpressionsTests
{
    [TestMethod]
    public async Task ExpressionsInStepArgumentTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.CSharpExpressions<CSharpExpressionsGlobals1>();

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.Then("then1 @Value");
            });
            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(value);
            });
        });

        Assert.IsTrue(traces is ["ExpressionValue"]);
    }

    [TestMethod]
    public async Task ExpressionsInDataTableTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.CSharpExpressions<CSharpExpressionsGlobals1>();

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.Then("given", (object?)new string[][] { ["test", "@Value"] });
            });
            services.Then(new("given"), (string[][] value) =>
            {
                traces.Add(value);
            });
        });

        Assert.IsTrue(traces is [string[][] and [["test", "ExpressionValue"]]]);
    }
}

public sealed class CSharpExpressionsGlobals1
{
    public string Value { get; } = "ExpressionValue";
}
