namespace BddDotNet.Extensibility;

public interface IArgumentTransformation
{
    ValueTask<object?> TransformAsync(object? input, Type targetType);
}
