namespace BddDotNet.Arguments;

public interface IArgumentTransformation
{
    ValueTask<object?> TransformAsync(object? input, Type targetType);
}
