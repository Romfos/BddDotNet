using BddDotNet.Extensibility;

namespace BddDotNet.Tests.Extensibility;

internal sealed class ArgumentTransformation(List<object?> traces) : IArgumentTransformation
{
    public object? Transform(object? input, Type targetType)
    {
        traces.Add(input);
        return targetType.FullName;
    }

    public ValueTask<object?> TransformAsync(object? input, Type targetType)
    {
        traces.Add(input);
        return new(targetType.FullName);
    }
}
