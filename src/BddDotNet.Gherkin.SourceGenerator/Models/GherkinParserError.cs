namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed class GherkinParserError(string message, string featureFilePath, int Line, int Column)
{
    public string Message { get; } = message;
    public string FeatureFilePath { get; } = featureFilePath;
    public int Line { get; } = Line;
    public int Column { get; } = Column;
}
