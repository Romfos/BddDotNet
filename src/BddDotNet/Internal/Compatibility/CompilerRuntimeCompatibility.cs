#if NETFRAMEWORK

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System.Runtime.CompilerServices;

[global::System.AttributeUsage(
    global::System.AttributeTargets.Class |
    global::System.AttributeTargets.Struct |
    global::System.AttributeTargets.Field |
    global::System.AttributeTargets.Property,
    AllowMultiple = false,
    Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal sealed class RequiredMemberAttribute : global::System.Attribute
{
}

/// <summary>
/// Indicates that compiler support for a particular feature is required for the location where this attribute is applied.
/// </summary>
/// <remarks>
/// Creates a new instance of the <see cref="global::System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute"/> type.
/// </remarks>
/// <param name="featureName">The name of the feature to indicate.</param>
[global::System.AttributeUsage(global::System.AttributeTargets.All, AllowMultiple = true, Inherited = false)]
[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
internal sealed class CompilerFeatureRequiredAttribute(string featureName) : global::System.Attribute
{

    /// <summary>
    /// The name of the compiler feature.
    /// </summary>
    public string FeatureName { get; } = featureName;

    /// <summary>
    /// If true, the compiler can choose to allow access to the location where this attribute is applied if it does not understand <see cref="FeatureName"/>.
    /// </summary>
    public bool IsOptional { get; set; }

    /// <summary>
    /// The <see cref="FeatureName"/> used for the ref structs C# feature.
    /// </summary>
    public const string RefStructs = nameof(RefStructs);

    /// <summary>
    /// The <see cref="FeatureName"/> used for the required members C# feature.
    /// </summary>
    public const string RequiredMembers = nameof(RequiredMembers);
}

#endif