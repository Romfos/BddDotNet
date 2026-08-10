using BddDotNet.Scenarios;

namespace BddDotNet.Internal.Models;

internal sealed record Scenario(
    string AssemblyName,
    string Namespace,
    string Feature,
    string Name,
    Func<IScenarioService, Task> Method,
    string FilePath,
    int LineNumber);