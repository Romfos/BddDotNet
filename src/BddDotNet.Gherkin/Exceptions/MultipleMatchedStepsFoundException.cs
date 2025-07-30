using BddDotNet.Gherkin.Models;

namespace BddDotNet.Gherkin.Exceptions;

public sealed class MultipleMatchedStepsFoundException : Exception
{
    internal MultipleMatchedStepsFoundException(StepType stepType, string text)
        : base($"Multiple matched steps found for {stepType} {text}")
    {
    }
}

