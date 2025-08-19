namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed class TestCaseParserResult
{
    public List<TestCaseBackground> Backgrounds { get; } = [];
    public List<TestCase> TestCases { get; } = [];
    public List<TestCasesParserError> Errors { get; } = [];
}
