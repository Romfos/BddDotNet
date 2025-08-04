using BddDotNet.Extensibility;
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
                traces.Add(2);
                await context.Then("then1");
                traces.Add(4);
            });

            services.BeforeScenario<LifecycleHooks>();
            services.AfterScenario<LifecycleHooks>();

            services.Then(new("then1"), () =>
            {
                traces.Add(3);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3, 4, null]);
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
                traces.Add(2);
                await context.Then("then1");
                traces.Add(4);
            });

            services.BeforeScenario<LifecycleHooks>();
            services.AfterScenario<LifecycleHooks>();

            services.Then(new("then1"), () =>
            {
                throw new Exception("Test exception");
            });
        });

        Assert.IsTrue(traces is [1, 2, "Test exception"]);
    }

    private class LifecycleHooks(List<object?> traces) : IBeforeScenario, IAfterScenario
    {
        public Task BeforeScenario()
        {
            traces.Add(1);
            return Task.CompletedTask;
        }

        public Task AfterScenario(Exception? exception)
        {
            traces.Add(exception?.Message);
            return Task.CompletedTask;
        }
    }
}
