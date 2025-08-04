using BddDotNet.Extensibility;

namespace BddDotNet.Tests.Extensibility;

internal sealed class ArgumentTransformation(List<object?> traces) : IArgumentTransformation
{
    public bool TryParse(object? input, Type type, out object? output)
    {
        traces.Add(input);
        output = type.FullName;
        return true;
    }
}
