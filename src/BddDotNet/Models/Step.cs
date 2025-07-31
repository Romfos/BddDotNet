using System.Text.RegularExpressions;

namespace BddDotNet.Models;

internal sealed class Step(
    StepType stepType,
    Regex pattern,
    Delegate body)
{
    public StepType StepType { get; } = stepType;
    public Regex Pattern { get; } = pattern;
    public Delegate Body { get; } = body;
}

