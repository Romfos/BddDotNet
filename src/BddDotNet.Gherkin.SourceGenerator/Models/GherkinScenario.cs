namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed record GherkinScenario(string AssemblyName, string Feature, string Scenario, string FeaturePath, int Line, GherkinBackground? Background)
{
    public string AssemblyName { get; } = AssemblyName;
    public string Feature { get; } = Feature;
    public string Scenario { get; set; } = Scenario;
    public string FeaturePath { get; } = FeaturePath;
    public int Line { get; } = Line;

    public GherkinBackground? Background { get; } = Background;
    public List<GherkinStep> Steps { get; set; } = [];
}