namespace BddDotNet.Components.Composition;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionsAttribute(object value) : Attribute
{
    public object Value { get; } = value;
}
