namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed record TestCaseStep(string Keyword, string Text, string[][]? DataTable, string? DocString, string FeaturePath, int Line, int Column)
{
    public string Keyword { get; } = Keyword;
    public string Text { get; set; } = Text;
    public string[][]? DataTable { get; set; } = DataTable;
    public string? DocString { get; set; } = DocString;
    public string FeaturePath { get; } = FeaturePath;
    public int Line { get; } = Line;
    public int Column { get; } = Column;
}
