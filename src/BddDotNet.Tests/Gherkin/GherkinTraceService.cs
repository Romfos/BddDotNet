using BddDotNet.Scenarios;

namespace BddDotNet.Tests.Gherkin;

internal sealed class GherkinTraceService(Dictionary<string, List<object>> traces) : IBeforeScenario, IAfterScenario
{
    private string? scenario;

    public void Trace(object trace)
    {
        if (scenario == null)
        {
            throw new NotImplementedException();
        }

        if (!traces.TryGetValue(scenario, out var scenarioTraces))
        {
            scenarioTraces = [];
            traces.Add(scenario, scenarioTraces);
        }
        scenarioTraces.Add(trace);
    }

    Task IAfterScenario.AfterScenarioAsync(ScenarioContext context, Exception? exception)
    {
        scenario = null;
        return Task.CompletedTask;
    }

    Task IBeforeScenario.BeforeScenarioAsync(ScenarioContext context)
    {
        scenario = context.Scenario;
        return Task.CompletedTask;
    }
}

