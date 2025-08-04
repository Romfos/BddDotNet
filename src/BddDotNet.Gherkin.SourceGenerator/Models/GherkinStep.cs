namespace BddDotNet.Gherkin.SourceGenerator.Models;

internal sealed record GherkinStep(string Pattern, string ServiceTypeName, string MethodName)
{
    public string Pattern { get; } = Pattern;
    public string ServiceTypeName { get; } = ServiceTypeName;
    public string MethodName { get; } = MethodName;
}
