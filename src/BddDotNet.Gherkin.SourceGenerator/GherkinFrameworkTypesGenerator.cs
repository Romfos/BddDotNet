using Microsoft.CodeAnalysis;

namespace BddDotNet.Gherkin.SourceGenerator;

[Generator]
internal sealed class GherkinFrameworkTypesGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
            .Select(static (compilation, _) => compilation.Assembly.Name);

        context.RegisterSourceOutput(assemblyName, static (context, assemblyName) =>
        {
            context.AddSource("GherkinGeneratorDiscoveredStepsDeclaration.cs",
                $$"""
                using Microsoft.Extensions.DependencyInjection;
                
                namespace {{assemblyName}};
                    
                public static partial class GherkinGeneratorDiscoveredSteps
                {
                    public static partial void AddDiscoveredSteps(this IServiceCollection services);
                }
                """);

            context.AddSource("GherkinGeneratedFeaturesExtensionsDeclaration.cs",
                $$"""
                using Microsoft.Extensions.DependencyInjection;
                
                namespace {{assemblyName}};
                    
                public static partial class GherkinGeneratedFeaturesExtensions
                {
                    public static partial void AddGeneratedFeatures(this IServiceCollection services);
                }
                """);
        });

        context.RegisterPostInitializationOutput(context =>
        {
            context.AddSource("GherkinAttributes.cs",
                $$"""
                namespace BddDotNet.Gherkin.SourceGenerator;
                
                internal sealed class GivenAttribute(string pattern) : System.Attribute
                {
                    public string Pattern { get; } = pattern;
                }

                internal sealed class WhenAttribute(string pattern) : System.Attribute
                {
                    public string Pattern { get; } = pattern;
                }

                internal sealed class ThenAttribute(string pattern) : System.Attribute
                {
                    public string Pattern { get; } = pattern;
                }
                """);
        });
    }
}
