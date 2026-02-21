using BddDotNet.Gherkin.CSharpExpressions;
using BddDotNet.Scenarios;
using BddDotNet.Steps;
using BddDotNet.Tests.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.CSharpExpressions;

[TestClass]
public sealed class CSharpExpressionsTests
{
    [TestMethod]
    public async Task StepArgumentWithExpressionsTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.CSharpExpressions<CSharpExpressionsGlobals1>();

            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(value);
            });

            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("then1 @Value");
            });
        });

        Assert.IsTrue(traces is ["ExpressionValue"]);
    }

    [TestMethod]
    public async Task DataTableWithExpressionsTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.CSharpExpressions<CSharpExpressionsGlobals1>();

            services.Then(new("given"), (string[][] value) =>
            {
                traces.Add(value);
            });

            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("given", (object?)new string[][] { ["test", "@Value"] });
            });
        });

        Assert.IsTrue(traces is [string[][] and [["test", "ExpressionValue"]]]);
    }
}

public sealed class CSharpExpressionsGlobals1
{
    public string Value { get; } = "ExpressionValue";
}
