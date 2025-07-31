using BddDotNet.Extensibility;
using BddDotNet.Models;

namespace BddDotNet.Services;

internal sealed class ScenarioExecutionService(IScenarioContext scenarioContext, IEnumerable<IBeforeScenario> beforeScenarioHooks)
{
    public async Task ExecuteAsync(Scenario scenario)
    {
        foreach (var beforeScenarioHook in beforeScenarioHooks)
        {
            await beforeScenarioHook.BeforeScenario();
        }

        await scenario.Method(scenarioContext);
    }
}
