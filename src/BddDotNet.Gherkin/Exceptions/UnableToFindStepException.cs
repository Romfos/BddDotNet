using BddDotNet.Gherkin.Models;

namespace BddDotNet.Gherkin.Exceptions;

public sealed class UnableToFindStepException : Exception
{
    internal UnableToFindStepException(StepType stepType, string text) : base($"Unable to find step to match {stepType} {text}")
    {
    }
}
