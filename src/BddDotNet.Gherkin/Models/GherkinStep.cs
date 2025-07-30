using System.Text.RegularExpressions;

namespace BddDotNet.Gherkin.Models;

internal sealed class GherkinStep(
    StepType stepType,
    Regex pattern,
    Delegate body)
{
    public StepType StepType { get; } = stepType;
    public Regex Pattern { get; } = pattern;
    public Delegate Body { get; } = body;
}

