namespace BddDotNet.Components;

internal static class StringPathExtensions
{
    public static string SanitizePath(this string path)
    {
        return new string(path.Where(x => !char.IsWhiteSpace(x)).ToArray());
    }
}
