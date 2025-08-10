namespace BddDotNet.Components.Routing.Internal.Extensions;

internal static class PathExtensions
{
    public static string GetPathKey(this string path)
    {
        return new string(path.Where(x => !char.IsWhiteSpace(x)).ToArray());
    }
}
