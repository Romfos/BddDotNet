using BddDotNet.Steps;
using System.Text.RegularExpressions;

namespace BddDotNet.Internal.Models;

internal sealed class Step(
    StepType stepType,
    Regex pattern,
    Func<IServiceProvider, Delegate> handlerFactory)
{
    public StepType StepType { get; } = stepType;
    public Regex Pattern { get; } = pattern;
    public Func<IServiceProvider, Delegate> HandlerFactory { get; } = handlerFactory;
}

