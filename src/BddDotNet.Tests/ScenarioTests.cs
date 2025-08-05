using BddDotNet.CSharpExpressions;
using BddDotNet.Tests.Extensibility;
using BddDotNet.Tests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests;

[TestClass]
public sealed class ScenarioTests
{
    [TestMethod]
    public async Task GivenStepTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Given("given1");
                traces.Add(3);
            });

            services.Given(new("given1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task WhenStepTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.When("when1");
                traces.Add(3);
            });

            services.When(new("when1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task ThenStepTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(3);
            });

            services.Then(new("then1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task ScenarioHooksTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(3);
            });

            services.BeforeScenario<ScenarioLifecycleHooks>();
            services.AfterScenario<ScenarioLifecycleHooks>();

            services.Then(new("then1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is ["BeforeScenario", 1, 2, 3, null]);
    }

    [TestMethod]
    public async Task ScenarioHooksWithExceptionTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(2);
            });

            services.BeforeScenario<ScenarioLifecycleHooks>();
            services.AfterScenario<ScenarioLifecycleHooks>();

            services.Then(new("then1"), () =>
            {
                throw new Exception("Test exception");
            });
        });

        Assert.IsTrue(traces is ["BeforeScenario", 1, "Test exception"]);
    }

    [TestMethod]
    public async Task ArgumentTransformationTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1 abcd");
                traces.Add(2);
            });

            services.ArgumentTransformation<ArgumentTransformation>();

            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(value);
            });
        });

        Assert.IsTrue(traces is [1, "abcd", "System.String", 2]);
    }

    [TestMethod]
    public async Task CSharpExpressionsTest()
    {
        var traces = new List<object?>();

        await Platform.RunTestAsync(services =>
        {
            services.AddCSharpExpressions<CSharpExpressionsGlobals>();

            services.AddSingleton(traces);

            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
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

        await Platform.RunTestAsync(services =>
        {
            services.AddCSharpExpressions<CSharpExpressionsGlobals>();

            services.AddSingleton(traces);

            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
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
