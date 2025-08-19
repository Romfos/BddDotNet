namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed record TestCase(string AssemblyName, string Feature, string Scenario, string FeaturePath, int Line, TestCaseBackground? Background)
{
    public string AssemblyName { get; } = AssemblyName;
    public string Feature { get; } = Feature;
    public string Scenario { get; set; } = Scenario;
    public string FeaturePath { get; } = FeaturePath;
    public int Line { get; } = Line;

    public TestCaseBackground? Background { get; } = Background;
    public List<TestCaseStep> Steps { get; set; } = [];
}