namespace BddDotNet.Gherkin.SourceGenerator.TestCases;

internal sealed record TestCaseStep(string Keyword, string Text, string[][]? DataTable, string FeaturePath, int Line, int Column)
{
    public string Keyword { get; } = Keyword;
    public string Text { get; set; } = Text;
    public string[][]? DataTable { get; set; } = DataTable;
    public string FeaturePath { get; } = FeaturePath;
    public int Line { get; } = Line;
    public int Column { get; } = Column;
}
