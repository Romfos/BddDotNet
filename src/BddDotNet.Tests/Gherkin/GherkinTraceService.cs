using BddDotNet.Extensibility;

namespace BddDotNet.Tests.Gherkin;

internal sealed class GherkinTraceService(ITestContext testContext, Dictionary<string, List<object>> traces)
{
    public void Trace(object trace)
    {
        if (!traces.TryGetValue(testContext.Scenario, out var scenarioTraces))
        {
            scenarioTraces = [];
            traces.Add(testContext.Scenario, scenarioTraces);
        }
        scenarioTraces.Add(trace);
    }
}

