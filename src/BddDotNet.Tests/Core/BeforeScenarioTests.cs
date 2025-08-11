using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Core;

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

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", context => Task.CompletedTask);

            services.BeforeScenario<BeforeScenario1>();
            services.BeforeScenario<BeforeScenario2>();
        });

        Assert.IsTrue(traces is [1, 2]);
    }
}

file sealed class BeforeScenario1(List<object?> traces) : IBeforeScenario
{
    public Task BeforeScenario()
    {
        traces.Add(1);
        return Task.CompletedTask;
    }
}

file sealed class BeforeScenario2(List<object?> traces) : IBeforeScenario
{
    public Task BeforeScenario()
    {
        traces.Add(2);
        return Task.CompletedTask;
    }
}