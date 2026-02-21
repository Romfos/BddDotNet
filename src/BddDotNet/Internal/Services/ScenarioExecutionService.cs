using BddDotNet.Internal.Models;
using BddDotNet.Scenarios;

namespace BddDotNet.Internal.Services;

internal sealed class ScenarioExecutionService(
    IScenarioService scenarioService,
    IEnumerable<IBeforeScenario> beforeScenarioHooks,
    IEnumerable<IAfterScenario> afterScenarioHooks)
{
    public async Task ExecuteAsync(Scenario scenario)
    {
        var scenarioContext = new ScenarioContext(scenario.Feature, scenario.Name);

        await RunBeforeScenarioHooksAsync(scenarioContext);

        try
        {
            await scenario.Method(scenarioService);
        }
        catch (Exception exception)
        {
            await RunAfterScenarioHooksAsync(scenarioContext, exception.GetBaseException());

            throw;
        }

        await RunAfterScenarioHooksAsync(scenarioContext, null);
    }

    private async Task RunBeforeScenarioHooksAsync(ScenarioContext scenarioContext)
    {
        foreach (var beforeScenarioHook in beforeScenarioHooks)
        {
            await beforeScenarioHook.BeforeScenarioAsync(scenarioContext);
        }
    }

    private async Task RunAfterScenarioHooksAsync(ScenarioContext scenarioContext, Exception? exception)
    {
        foreach (var afterScenarioHook in afterScenarioHooks.Reverse())
        {
            await afterScenarioHook.AfterScenarioAsync(scenarioContext, exception);
        }
    }
}
