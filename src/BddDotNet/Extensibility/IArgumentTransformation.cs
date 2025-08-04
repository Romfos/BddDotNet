namespace BddDotNet.Extensibility;

public interface IArgumentTransformation
{
    bool TryParse(object? input, Type targetType, out object? output);
}
