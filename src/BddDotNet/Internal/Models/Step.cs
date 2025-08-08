using System.Text.RegularExpressions;

namespace BddDotNet.Internal.Models;

internal sealed class Step(
    StepType stepType,
    Regex pattern,
    Func<IServiceProvider, Delegate> factory)
{
    public StepType StepType { get; } = stepType;
    public Regex Pattern { get; } = pattern;
    public Func<IServiceProvider, Delegate> Factory { get; } = factory;
}

