using BddDotNet.Extensibility;

namespace BddDotNet.Tests.Extensibility;

internal sealed class ScenarioLifecycleHooks(List<object?> traces) : IBeforeScenario, IAfterScenario
{
    public Task BeforeScenario()
    {
        traces.Add("BeforeScenario");
        return Task.CompletedTask;
    }

    public Task AfterScenario(Exception? exception)
    {
        traces.Add(exception?.Message);
        return Task.CompletedTask;
    }
}
