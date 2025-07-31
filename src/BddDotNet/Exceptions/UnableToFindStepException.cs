using BddDotNet.Models;

namespace BddDotNet.Exceptions;

internal sealed class UnableToFindStepException(StepType stepType, string text)
    : Exception($"Unable to find step to match {stepType} {text}")
{
}
