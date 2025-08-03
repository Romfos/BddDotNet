using Gherkin;
using Gherkin.Ast;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Text;

namespace BddDotNet.Gherkin.SourceGenerator;

[Generator]
internal sealed class ScenariosExtensionsGenerator : IIncrementalGenerator
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
        context.AddSource($"SourceGeneratedGherkinScenarios.g.cs", formattedFeatureClassContent);
    }

    private static string GetTestClassContent(string assemblyName, ImmutableArray<AdditionalText> features)
    {
        var featureClassContent =
           $$"""
            using BddDotNet;
            using Microsoft.Extensions.DependencyInjection;

            namespace {{assemblyName}};

            public static partial class GherkinSourceGeneratorExtensions
            {
                public static partial void SourceGeneratedGherkinScenarios(this IServiceCollection services)
                {
                    {{GetFeaturesDeclarations(assemblyName, features)}}
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

    private static string GetFeaturesDeclarations(string assemblyName, ImmutableArray<AdditionalText> features)
    {
        var declarations = new StringBuilder();

        foreach (var feature in features)
        {
            using var stringReader = new StringReader(feature.GetText()?.ToString());
            var gherkinDocument = new Parser().Parse(stringReader);

            foreach (var scenario in gherkinDocument.Feature.Children.OfType<Scenario>().Where(x => !x.Examples.Any()))
            {
                declarations.AppendLine(GetScenarioDeclaration(assemblyName, feature.Path, gherkinDocument.Feature.Name, scenario));
            }
        }

        return declarations.ToString();
    }

    private static string GetScenarioDeclaration(string assemblyName, string featurePath, string featureName, Scenario scenario)
    {
        var declaration =
            $$""""
            services.Scenario(
                "{{assemblyName}}",
                "{{assemblyName}}",
                "{{featureName}}",
                "{{scenario.Name}}",
                """{{featurePath}}""",
                {{scenario.Location.Line}},
                async context =>
                {
                    {{GetScenarioStepsDeclarations(featurePath, scenario)}}
                });
            """";

        return declaration;
    }

    private static string GetScenarioStepsDeclarations(string featurePath, Scenario scenario)
    {
        var stepDeclarations = new StringBuilder();

        var keyword = "Given";

        foreach (var step in scenario.Steps)
        {
            if (!step.Keyword.StartsWith("And"))
            {
                keyword = step.Keyword;
            }

            stepDeclarations.AppendLine(
                $""""
                #line ({step.Location.Line}, {step.Location.Column})-({step.Location.Line + 1}, 1) 12 "{featurePath}"
                """");

            if (step.Argument is DataTable dataTable)
            {
                stepDeclarations.AppendLine(
                    $$""""
                    await context.{{keyword}}("""{{step.Text}}""", (object)new string[][]
                    {
                        {{GetDataTableDeclaration(dataTable)}}
                    });
                    """");
            }
            else if (step.Argument is DocString docString)
            {
                throw new NotImplementedException("DocString are not supported for now");
            }
            else if (step.Argument is null)
            {
                stepDeclarations.AppendLine(
                    $""""
                    await context.{keyword}("""{step.Text}""");
                    """");
            }
            else
            {
                throw new NotImplementedException($"Unsupported step argument type: {step.Argument?.GetType().FullName}");
            }
        }

        stepDeclarations.AppendLine("#line default");

        return stepDeclarations.ToString();
    }

    private static string GetDataTableDeclaration(DataTable dataTable)
    {
        var dataTableDeclaration = new StringBuilder();

        foreach (var row in dataTable.Rows)
        {
            dataTableDeclaration.Append("new[] {");
            foreach (var cell in row.Cells)
            {
                dataTableDeclaration.Append(
                    $""""
                    """{cell.Value}""",
                    """");
            }
            dataTableDeclaration.AppendLine("},");
        }

        return dataTableDeclaration.ToString();
    }
}
