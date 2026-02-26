using BddDotNet.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Scenarios;

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
            // reverse order of execution is expected here
            services.AfterScenario<AfterScenario2>();
            services.AfterScenario<AfterScenario1>();

            services.Scenario("feature1", "scenario1", scenario => Task.CompletedTask);
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
            // reverse order of execution is expected here
            services.AfterScenario<AfterScenario1>();
            services.AfterScenario<AfterScenario2>();

            services.Scenario("feature1", "scenario1", async scenario =>
            {
                throw new Exception("exception1");
            });
        });

        Assert.IsTrue(traces is [2, "exception1", 1, "exception1"]);
    }
}

file sealed class AfterScenario1(List<object?> traces) : IAfterScenario
{
    public Task AfterScenarioAsync(ScenarioContext context, Exception? exception)
    {
        traces.Add(1);
        traces.Add(exception?.Message);
        return Task.CompletedTask;
    }
}

file sealed class AfterScenario2(List<object?> traces) : IAfterScenario
{
    public Task AfterScenarioAsync(ScenarioContext context, Exception? exception)
    {
        traces.Add(2);
        traces.Add(exception?.Message);
        return Task.CompletedTask;
    }
}
