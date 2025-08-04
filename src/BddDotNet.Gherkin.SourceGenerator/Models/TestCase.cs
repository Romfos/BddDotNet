namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed record TestCase(string AssemblyName, string Feature, string Scenario, string FeaturePath, int Line)
{
    public string AssemblyName { get; } = AssemblyName;
    public string Feature { get; } = Feature;
    public string Scenario { get; set; } = Scenario;
    public string FeaturePath { get; } = FeaturePath;
    public int Line { get; } = Line;

    public List<TestCaseStep> Steps { get; set; } = [];
}