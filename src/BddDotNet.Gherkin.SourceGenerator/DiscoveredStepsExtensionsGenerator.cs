using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace BddDotNet.Gherkin.SourceGenerator;

[Generator]
internal sealed class DiscoveredStepsExtensionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
            .Select(static (compilation, _) => compilation.Assembly.Name);

        var givenStepDeclarationNodes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "BddDotNet.Gherkin.SourceGenerator.GivenAttribute",
                static (node, _) => node is MethodDeclarationSyntax && node.Parent is ClassDeclarationSyntax,
                GetMetadataForStep)
            .Collect();

        var whenStepDeclarationNodes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "BddDotNet.Gherkin.SourceGenerator.WhenAttribute",
                static (node, _) => node is MethodDeclarationSyntax && node.Parent is ClassDeclarationSyntax,
                GetMetadataForStep)
            .Collect();

        var thenStepDeclarationNodes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "BddDotNet.Gherkin.SourceGenerator.ThenAttribute",
                static (node, _) => node is MethodDeclarationSyntax && node.Parent is ClassDeclarationSyntax,
                GetMetadataForStep)
            .Collect();

        var arguments = givenStepDeclarationNodes
            .Combine(whenStepDeclarationNodes)
            .Combine(thenStepDeclarationNodes)
            .Combine(assemblyName);

        context.RegisterImplementationSourceOutput(arguments, static (context, args) =>
        {
            var (((givenNodes, whenNodes), thenNodes), assemblyName) = args;

            var stepDeclarations = new StringBuilder();

            AppendStepClassRegistrations(stepDeclarations, givenNodes.Concat(whenNodes).Concat(thenNodes));
            AppendStepDeclarations(stepDeclarations, "Given", givenNodes);
            AppendStepDeclarations(stepDeclarations, "When", whenNodes);
            AppendStepDeclarations(stepDeclarations, "Then", thenNodes);

            var content = GenerateClass(assemblyName, stepDeclarations);
            var formattedContent = FormatCode(content);

            context.AddSource("GherkinGeneratorDiscoveredSteps.cs", formattedContent);
        });
    }

    private static void AppendStepClassRegistrations(StringBuilder output, IEnumerable<(string, string className, string)> definitions)
    {
        var types = definitions.Select(def => def.className).Distinct();

        foreach (var type in types)
        {
            output.AppendLine(
                $$"""
                services.TryAddScoped<{{type}}>();
                """);
        }
    }

    private static void AppendStepDeclarations(StringBuilder output, string extensionMethodName, IEnumerable<(string, string, string)> definitions)
    {
        foreach (var (pattern, className, methodName) in definitions)
        {
            output.AppendLine(
                $$"""
                services.{{extensionMethodName}}(new("{{pattern}}"), async (IServiceProvider services) =>
                {
                    await services.GetRequiredService<{{className}}>().{{methodName}}();
                });
                """);
        }
    }

    private static string GenerateClass(string assemblyName, StringBuilder stepDeclarations)
    {
        var content =
            $$"""
            using BddDotNet.Gherkin.Extensions;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

            namespace {{assemblyName}};
                
            public static partial class GherkinGeneratorDiscoveredSteps
            {
                public static partial void AddDiscoveredSteps(this IServiceCollection services)
                {
                    {{stepDeclarations}}
                }
            }
            """;

        return content;
    }

    public static string FormatCode(string code)
    {
        return CSharpSyntaxTree.ParseText(code)
            .GetRoot()
            .NormalizeWhitespace()
            .SyntaxTree
            .GetText()
            .ToString();
    }

    private static (string, string, string) GetMetadataForStep(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.TargetNode;
        var classDeclaration = (ClassDeclarationSyntax)methodDeclaration.Parent!;

        var typeName = context.SemanticModel.GetDeclaredSymbol(classDeclaration)!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var methodName = context.SemanticModel.GetDeclaredSymbol(methodDeclaration)!.Name;

        var pattern = context.Attributes
            .Select(attr => (string)attr.ConstructorArguments[0].Value!)
            .Single();

        return (pattern, typeName, methodName);
    }
}
