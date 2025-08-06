using BddDotNet.Extensibility;

namespace BddDotNet.Tests.Core;

internal sealed class TestArgumentTransformation(List<object?> traces) : IArgumentTransformation
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
