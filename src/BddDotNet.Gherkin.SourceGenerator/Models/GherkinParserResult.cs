namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed class GherkinParserResult
{
    public List<GherkinBackground> Backgrounds { get; } = [];
    public List<GherkinScenario> Scenarios { get; } = [];
    public List<GherkinParserError> Errors { get; } = [];
}
