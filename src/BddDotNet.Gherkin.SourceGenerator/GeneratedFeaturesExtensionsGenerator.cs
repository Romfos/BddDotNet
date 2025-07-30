using Gherkin;
using Gherkin.Ast;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Text;

namespace BddDotNet.Gherkin.SourceGenerator;

[Generator]
internal sealed class GeneratedFeaturesExtensionsGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var arguments = context.CompilationProvider
            .Select(static (compilation, _) => compilation.Assembly.Name);

        var featureFiles = context.AdditionalTextsProvider
            .Where(static file => Path.GetExtension(file.Path) == ".feature")
            .Collect();

        context.RegisterImplementationSourceOutput(featureFiles.Combine(arguments), GenerateFeatureClass);
    }

    private static void GenerateFeatureClass(SourceProductionContext context, (ImmutableArray<AdditionalText> features, string? assemblyName) args)
    {
        if (args.assemblyName == null)
        {
            return;
        }

        var featureClassContent = GetTestClassContent(args.assemblyName, args.features);
        var formattedFeatureClassContent = FormatCode(featureClassContent);
        context.AddSource($"GherkinGeneratedFeaturesExtensions.g.cs", formattedFeatureClassContent);
    }

    private static string GetTestClassContent(string assemblyName, ImmutableArray<AdditionalText> features)
    {
        var declarations = new StringBuilder();

        foreach (var feature in features)
        {
            ProcessFeature(declarations, assemblyName, feature);
        }

        var featureClassContent =
           $$"""
            using Microsoft.Extensions.DependencyInjection;
            using BddDotNet.Gherkin.Extensions;
            using BddDotNet.Gherkin.Services;

            namespace {{assemblyName}};

            public static partial class GherkinGeneratedFeaturesExtensions
            {
                public static partial void AddGeneratedFeatures(this IServiceCollection services)
                {
                    {{declarations}}
                }
            }
            """;

        return featureClassContent;
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

    private static void ProcessFeature(StringBuilder output, string assemblyName, AdditionalText feature)
    {
        using var stringReader = new StringReader(feature.GetText()?.ToString());
        var gherkinDocument = new Parser().Parse(stringReader);

        foreach (var scenario in gherkinDocument.Feature.Children.OfType<Scenario>())
        {
            ProcessScenario(output, assemblyName, feature.Path, gherkinDocument.Feature.Name, scenario);
        }
    }

    private static void ProcessScenario(StringBuilder output, string assemblyName, string featurePath, string featureName, Scenario scenario)
    {
        var declarations = new StringBuilder();

        foreach (var step in scenario.Steps)
        {
            ProcessStep(declarations, step);
        }

        output.AppendLine(
            $$""""
            services.Scenario("{{assemblyName}}", "{{featureName}}", "{{scenario.Name}}", """{{featurePath}}""", {{scenario.Location.Line}}, async context =>
            {
                {{declarations}}
            });
            """");
    }

    private static void ProcessStep(StringBuilder output, Step step)
    {
        output.AppendLine(
            $""""
            await context.{step.Keyword}("""{step.Text}""");
            """");
    }
}
