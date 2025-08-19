namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed class GherkinBackground(int index)
{
    public int Index { get; } = index;

    public List<GherkinStep> Steps { get; set; } = [];
}