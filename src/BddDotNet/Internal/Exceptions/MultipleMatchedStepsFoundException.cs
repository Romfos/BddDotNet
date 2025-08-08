using BddDotNet.Internal.Models;

namespace BddDotNet.Internal.Exceptions;

internal sealed class MultipleMatchedStepsFoundException(StepType stepType, string text)
    : Exception($"Multiple matched steps found for {stepType} {text}")
{
}

