namespace BddDotNet.Components.Routing.Internal.Exceptions;

internal sealed class UnsupportedContractException(string path, string typeName)
    : Exception($"Component at path '{path}' is not implementing '{typeName}'")
{
}