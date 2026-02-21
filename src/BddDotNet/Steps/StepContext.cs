using System.Text.RegularExpressions;

namespace BddDotNet.Steps;

public sealed record StepContext
{
    public StepType Type { get; }
    public string Text { get; }
    public Regex Pattern { get; }
    public object?[] Arguments { get; }

    internal StepContext(StepType type, string text, Regex pattern, object?[] arguments)
    {
        Type = type;
        Text = text;
        Pattern = pattern;
        Arguments = arguments;
    }
}
