namespace BddDotNet.Steps;

public sealed record StepFallbackContext
{
    public StepType Type { get; }
    public string Text { get; }
    public object?[] AdditionalStepArguments { get; }

    internal StepFallbackContext(StepType type, string text, object?[] additionalStepArguments)
    {
        Type = type;
        Text = text;
        AdditionalStepArguments = additionalStepArguments;
    }
}
