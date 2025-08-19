namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed class TestCaseBackground(int index)
{
    public int Index { get; } = index;

    public List<TestCaseStep> Steps { get; set; } = [];
}