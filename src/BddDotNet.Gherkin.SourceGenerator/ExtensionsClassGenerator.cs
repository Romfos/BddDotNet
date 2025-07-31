using Microsoft.CodeAnalysis;

namespace BddDotNet.Gherkin.SourceGenerator;

[Generator]
internal sealed class ExtensionsClassGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
            .Select(static (compilation, _) => compilation.Assembly.Name);

        context.RegisterSourceOutput(assemblyName, static (context, assemblyName) =>
        {
            context.AddSource("GherkinSourceGeneratorExtensions.cs",
                $$"""
                using Microsoft.Extensions.DependencyInjection;
                
                namespace {{assemblyName}};
                    
                public static partial class GherkinSourceGeneratorExtensions
                {
                    public static partial void SourceGeneratedGherkinSteps(this IServiceCollection services);
                
                    public static partial void SourceGeneratedGherkinScenarios(this IServiceCollection services);
                }
                """);
        });
    }
}
