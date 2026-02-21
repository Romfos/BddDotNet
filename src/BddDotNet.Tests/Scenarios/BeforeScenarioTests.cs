using BddDotNet.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Scenarios;

[TestClass]
public sealed class BeforeScenarioTests
{
    [TestMethod]
    public async Task BeforeScenarioTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.BeforeScenario<BeforeScenario1>();
            services.BeforeScenario<BeforeScenario2>();

            services.Scenario<ScenarioTests>("feature1", "scenario1", scenario => Task.CompletedTask);
        });

        Assert.IsTrue(traces is [1, 2]);
    }
}

file sealed class BeforeScenario1(List<object?> traces) : IBeforeScenario
{
    public Task BeforeScenarioAsync(ScenarioContext context)
    {
        traces.Add(1);
        return Task.CompletedTask;
    }
}

file sealed class BeforeScenario2(List<object?> traces) : IBeforeScenario
{
    public Task BeforeScenarioAsync(ScenarioContext context)
    {
        traces.Add(2);
        return Task.CompletedTask;
    }
}