using BddDotNet.Extensibility;

namespace BddDotNetAot.Transformations;

internal sealed class DictionaryArgumentTransformation : IArgumentTransformation
{
    public ValueTask<object?> TransformAsync(object? input, Type targetType)
    {
        if (input is string[][] dataTable && targetType == typeof(Dictionary<string, string>))
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var row in dataTable.Skip(1))
            {
                dictionary[row[0]] = row[1];
            }

            return new(dictionary);
        }

        return new(input);
    }
}
