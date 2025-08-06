using BddDotNet.CSharpExpressions;
using BddDotNet.Tests.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.CSharpExpressions;

[TestClass]
public sealed class CSharpExpressionsTests
{
    [TestMethod]
    public async Task CSharpExpressionsTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddCSharpExpressions<TestCSharpExpressionsGlobals>();

            services.AddSingleton(traces);

            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1 @Value");
                traces.Add(2);
            });

            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(value);
            });
        });

        Assert.IsTrue(traces is [1, "ExpressionValue", 2]);
    }

    [TestMethod]
    public async Task CSharpExpressionsForDataTableTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddCSharpExpressions<TestCSharpExpressionsGlobals>();

            services.AddSingleton(traces);

            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("given", (object?)new string[][] { ["test", "@Value"] });
                traces.Add(2);
            });

            services.Then(new("given"), (string[][] value) =>
            {
                traces.Add(value);
            });
        });

        Assert.IsTrue(traces is [1, string[][] and [["test", "ExpressionValue"]], 2]);
    }
}
