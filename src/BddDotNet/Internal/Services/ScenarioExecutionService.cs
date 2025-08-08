using BddDotNet.Extensibility;
using BddDotNet.Internal.Models;

namespace BddDotNet.Internal.Services;

internal sealed class ScenarioExecutionService(
    IScenarioContext scenarioContext,
    IEnumerable<IBeforeScenario> beforeScenarioHooks,
    IEnumerable<IAfterScenario> afterScenarioHooks)
{
    public async Task ExecuteAsync(Scenario scenario)
    {
        await RunBeforeScenarioHooksAsync();

        try
        {
            await scenario.Method(scenarioContext);
        }
        catch (Exception exception)
        {
            await RunAfterScenarioHooksAsync(exception.GetBaseException());

            throw;
        }

        await RunAfterScenarioHooksAsync(null);
    }

    private async Task RunBeforeScenarioHooksAsync()
    {
        foreach (var beforeScenarioHook in beforeScenarioHooks)
        {
            await beforeScenarioHook.BeforeScenario();
        }
    }

    private async Task RunAfterScenarioHooksAsync(Exception? exception)
    {
        foreach (var afterScenarioHook in afterScenarioHooks.Reverse())
        {
            await afterScenarioHook.AfterScenario(exception);
        }
    }
}
