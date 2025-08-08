using BddDotNet.Gherkin.SourceGenerator.Models;

namespace BddDotNet.Gherkin.SourceGenerator.Exceptions;

internal sealed class TestCasesParserException(List<TestCasesParserError> errors) : Exception
{
    public List<TestCasesParserError> Errors { get; } = errors;
}
