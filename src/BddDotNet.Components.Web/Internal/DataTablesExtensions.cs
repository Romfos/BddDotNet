namespace BddDotNet.Components.Web.Internal;

internal static class DataTablesExtensions
{
    internal static IEnumerable<(string, string)> ParseNameValueTable(this string[][] values)
    {
        if (!(values.Length > 1
            && values.All(x => x.Length == 2)
            && values[0][0] == "Name"
            && values[0][1] == "Value"))
        {
            throw new ArgumentException("Invalid table format. Name-Value table is expected with at least one row");
        }

        return values.Skip(1).Select(x => (x[0], x[1]));
    }

    internal static IEnumerable<string> ParseSingleColumnNameTable(this string[][] values)
    {
        if (!(values.Length > 1
            && values.All(x => x.Length == 1)
            && values[0][0] == "Name"))
        {
            throw new ArgumentException("Invalid table format. Single column table with Name header with at least one row is expected ");
        }

        return values.Skip(1).Select(x => x[0]);
    }
}
