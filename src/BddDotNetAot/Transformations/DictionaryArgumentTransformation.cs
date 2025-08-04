using BddDotNet.Extensibility;

namespace BddDotNetAot.Transformations;

internal sealed class DictionaryArgumentTransformation : IArgumentTransformation
{
    public bool TryParse(object? input, Type targetType, out object? output)
    {
        if (input is string[][] dataTable && targetType == typeof(Dictionary<string, string>))
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var row in dataTable.Skip(1))
            {
                dictionary[row[0]] = row[1];
            }

            output = dictionary;
            return true;
        }

        output = null;
        return false;
    }
}
