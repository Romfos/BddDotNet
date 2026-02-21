namespace BddDotNet.Scenarios;

public sealed record ScenarioContext
{
    public string Feature { get; }
    public string Scenario { get; }

    internal ScenarioContext(string feature, string scenario)
    {
        Feature = feature;
        Scenario = scenario;
    }
}
