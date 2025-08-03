using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Text;

namespace BddDotNet.Gherkin.SourceGenerator;

[Generator]
internal sealed class StepsExtensionsGenerator : IIncrementalGenerator
{
    private sealed record StepDefinition(string Pattern, string ServiceTypeName, string MethodName)
    {
        public string Pattern { get; } = Pattern;
        public string ServiceTypeName { get; } = ServiceTypeName;
        public string MethodName { get; } = MethodName;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var assemblyName = context.CompilationProvider
            .Select(static (compilation, _) => compilation.Assembly.Name);

        var givenStepDeclarationNodes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "BddDotNet.Gherkin.GivenAttribute",
                static (node, _) => node is MethodDeclarationSyntax && node.Parent is ClassDeclarationSyntax,
                GetMetadataForStep)
            .Collect();

        var whenStepDeclarationNodes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "BddDotNet.Gherkin.WhenAttribute",
                static (node, _) => node is MethodDeclarationSyntax && node.Parent is ClassDeclarationSyntax,
                GetMetadataForStep)
            .Collect();

        var thenStepDeclarationNodes = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "BddDotNet.Gherkin.ThenAttribute",
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

            var methodBodyContent = GetMethodBodyContent(givenNodes, whenNodes, thenNodes);
            var content = GenerateClassContent(assemblyName, methodBodyContent);
            var formattedContent = FormatCode(content);

            context.AddSource("SourceGeneratedGherkinSteps.cs", formattedContent);
        });
    }

    private static string GetMethodBodyContent(
        ImmutableArray<StepDefinition> givenSteps,
        ImmutableArray<StepDefinition> whenSteps,
        ImmutableArray<StepDefinition> thenSteps)
    {
        var methodBodyContent = new StringBuilder();

        AppendTypeRegistrations(methodBodyContent, givenSteps, whenSteps, thenSteps);

        AppendStepDeclarations(methodBodyContent, "Given", givenSteps);
        AppendStepDeclarations(methodBodyContent, "When", whenSteps);
        AppendStepDeclarations(methodBodyContent, "Then", thenSteps);

        return methodBodyContent.ToString();
    }

    private static void AppendTypeRegistrations(
        StringBuilder methodBodyContent,
        ImmutableArray<StepDefinition> givenSteps,
        ImmutableArray<StepDefinition> whenSteps,
        ImmutableArray<StepDefinition> thenSteps)
    {
        var serviceTypeNames = givenSteps
            .Concat(whenSteps)
            .Concat(thenSteps)
            .Select(x => x.ServiceTypeName)
            .Distinct();

        foreach (var serviceTypeName in serviceTypeNames)
        {
            methodBodyContent.AppendLine(
                $$"""
                services.TryAddScoped<{{serviceTypeName}}>();
                """);
        }
    }

    private static void AppendStepDeclarations(StringBuilder output, string extensionMethodName, ImmutableArray<StepDefinition> steps)
    {
        foreach (var step in steps)
        {
            output.AppendLine(
                $$"""
                services.{{extensionMethodName}}(
                    new("{{step.Pattern}}"),
                    services => services.GetRequiredService<{{step.ServiceTypeName}}>().{{step.MethodName}});
                """);
        }
    }

    private static string GenerateClassContent(string assemblyName, string methodBodyContent)
    {
        var classContent =
            $$"""
            using BddDotNet;
            using Microsoft.Extensions.DependencyInjection;
            using Microsoft.Extensions.DependencyInjection.Extensions;

            namespace {{assemblyName}};
                
            public static partial class GherkinSourceGeneratorExtensions
            {
                public static partial void SourceGeneratedGherkinSteps(this IServiceCollection services)
                {
                    {{methodBodyContent}}
                }
            }
            """;

        return classContent;
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

    private static StepDefinition GetMetadataForStep(GeneratorAttributeSyntaxContext context, CancellationToken cancellationToken)
    {
        var methodDeclaration = (MethodDeclarationSyntax)context.TargetNode;
        var classDeclaration = (ClassDeclarationSyntax)methodDeclaration.Parent!;

        var typeName = context.SemanticModel.GetDeclaredSymbol(classDeclaration)!.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        var methodName = context.SemanticModel.GetDeclaredSymbol(methodDeclaration)!.Name;

        var pattern = context.Attributes
            .Select(attr => (string)attr.ConstructorArguments[0].Value!)
            .Single();

        return new(pattern, typeName, methodName);
    }
}
