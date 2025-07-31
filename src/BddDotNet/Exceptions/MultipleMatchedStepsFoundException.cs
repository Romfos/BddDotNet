using BddDotNet.Models;

namespace BddDotNet.Exceptions;

internal sealed class MultipleMatchedStepsFoundException(StepType stepType, string text)
    : Exception($"Multiple matched steps found for {stepType} {text}")
{
}

