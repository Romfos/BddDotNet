using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Core;

[TestClass]
public sealed class AfterScenarioTests
{
    [TestMethod]
    public async Task AfterScenarioWithoutExceptionTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", context => Task.CompletedTask);

            // reverse order of execution is expected here
            services.AfterScenario<AfterScenario2>();
            services.AfterScenario<AfterScenario1>();
        });

        Assert.IsTrue(traces is [1, null, 2, null]);
    }

    [TestMethod]
    public async Task AfterScenarioWithExceptionTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.When("step1");
            });
            services.When(new("step1"), () => Task.FromException(new Exception("exception")));

            // reverse order of execution is expected here
            services.AfterScenario<AfterScenario1>();
            services.AfterScenario<AfterScenario2>();
        });

        Assert.IsTrue(traces is [2, "exception", 1, "exception"]);
    }
}

file sealed class AfterScenario1(List<object?> traces) : IAfterScenario
{
    public Task AfterScenario(Exception? exception)
    {
        traces.Add(1);
        traces.Add(exception?.Message);
        return Task.CompletedTask;
    }
}

file sealed class AfterScenario2(List<object?> traces) : IAfterScenario
{
    public Task AfterScenario(Exception? exception)
    {
        traces.Add(2);
        traces.Add(exception?.Message);
        return Task.CompletedTask;
    }
}
