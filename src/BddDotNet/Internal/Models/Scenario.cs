using BddDotNet.Extensibility;

namespace BddDotNet.Internal.Models;

internal sealed class Scenario(
    string assemblyName,
    string @namespace,
    string feature,
    string name,
    Func<IScenarioContext, Task> method,
    string filePath,
    int lineNumber)
{
    public string AssemblyName { get; } = assemblyName;
    public string Namespace { get; } = @namespace;
    public string Feature { get; } = feature;
    public string Name { get; } = name;
    public Func<IScenarioContext, Task> Method { get; } = method;
    public string FilePath { get; } = filePath;
    public int LineNumber { get; } = lineNumber;
}