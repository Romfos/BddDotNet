#if NETFRAMEWORK

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Diagnostics.CodeAnalysis;

//
// Summary:
//     Specifies the types of members that are dynamically accessed. This enumeration
//     has a System.FlagsAttribute attribute that allows a bitwise combination of its
//     member values.
[Flags]
internal enum DynamicallyAccessedMemberTypes
{
    //
    // Summary:
    //     Specifies all members.
    All = -1,
    //
    // Summary:
    //     Specifies no members.
    None = 0,
    //
    // Summary:
    //     Specifies the default, parameterless public constructor.
    PublicParameterlessConstructor = 1,
    //
    // Summary:
    //     Specifies all public constructors.
    PublicConstructors = 3,
    //
    // Summary:
    //     Specifies all non-public constructors.
    NonPublicConstructors = 4,
    //
    // Summary:
    //     Specifies all public methods.
    PublicMethods = 8,
    //
    // Summary:
    //     Specifies all non-public methods.
    NonPublicMethods = 16,
    //
    // Summary:
    //     Specifies all public fields.
    PublicFields = 32,
    //
    // Summary:
    //     Specifies all non-public fields.
    NonPublicFields = 64,
    //
    // Summary:
    //     Specifies all public nested types.
    PublicNestedTypes = 128,
    //
    // Summary:
    //     Specifies all non-public nested types.
    NonPublicNestedTypes = 256,
    //
    // Summary:
    //     Specifies all public properties.
    PublicProperties = 512,
    //
    // Summary:
    //     Specifies all non-public properties.
    NonPublicProperties = 1024,
    //
    // Summary:
    //     Specifies all public events.
    PublicEvents = 2048,
    //
    // Summary:
    //     Specifies all non-public events.
    NonPublicEvents = 4096,
    //
    // Summary:
    //     Specifies all interfaces implemented by the type.
    Interfaces = 8192
}

/// <summary>
/// Initializes a new instance of the <see cref="DynamicallyAccessedMembersAttribute"/> class
/// with the specified member types.
/// </summary>
/// <param name="memberTypes">The types of members dynamically accessed.</param>
internal sealed class DynamicallyAccessedMembersAttribute(DynamicallyAccessedMemberTypes memberTypes) : Attribute
{
    /// <summary>
    /// Gets the <see cref="DynamicallyAccessedMemberTypes"/> which specifies the type
    /// of members dynamically accessed.
    /// </summary>
    public DynamicallyAccessedMemberTypes MemberTypes { get; } = memberTypes;
}

//
// Summary:
//     Specifies that when a method returns System.Diagnostics.CodeAnalysis.NotNullWhenAttribute.ReturnValue,
//     the parameter will not be null even if the corresponding type allows it.
[AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
public sealed class NotNullWhenAttribute(bool returnValue) : Attribute
{
    //
    // Summary:
    //     Gets the return value condition.
    //
    // Returns:
    //     The return value condition. If the method returns this value, the associated
    //     parameter will not be null.
    public bool ReturnValue { get; } = returnValue;
}


#endif