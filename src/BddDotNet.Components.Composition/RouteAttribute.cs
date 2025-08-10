namespace BddDotNet.Components.Composition;

[AttributeUsage(AttributeTargets.Property)]
public sealed class RouteAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
