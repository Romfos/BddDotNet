using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Core;

[TestClass]
public sealed class BddDotNetTests
{
    [TestMethod]
    public async Task GivenStepTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
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

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
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

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
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

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(3);
            });

            services.BeforeScenario<TestScenarioHooks>();
            services.AfterScenario<TestScenarioHooks>();

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

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(2);
            });

            services.BeforeScenario<TestScenarioHooks>();
            services.AfterScenario<TestScenarioHooks>();

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

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<BddDotNetTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1 abcd");
                traces.Add(2);
            });

            services.ArgumentTransformation<TestArgumentTransformation>();

            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(value);
            });
        });

        Assert.IsTrue(traces is [1, "abcd", "System.String", 2]);
    }
}
