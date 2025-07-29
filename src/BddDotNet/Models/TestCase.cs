namespace BddDotNet.Models;

internal sealed class TestCase(
    string assemblyName,
    string @namespace,
    string testCaseTypeName,
    string testCaseName,
    Func<IServiceProvider, Task> method,
    string filePath,
    int lineNumber)
{
    public string AssemblyName { get; } = assemblyName;
    public string Namespace { get; } = @namespace;
    public string TestCaseTypeName { get; } = testCaseTypeName;
    public string TestCaseName { get; } = testCaseName;
    public Func<IServiceProvider, Task> Method { get; } = method;
    public string FilePath { get; } = filePath;
    public int LineNumber { get; } = lineNumber;
}