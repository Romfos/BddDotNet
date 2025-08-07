using BddDotNet.Gherkin.SourceGenerator.Models;
using BddDotNet.Gherkin.SourceGenerator.Services;
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

        var testCasesParser = new TestCasesParser();
        var testCases = testCasesParser.GetTestCases(args.assemblyName, args.features);
        var featureClassContent = GetTestClassContent(args.assemblyName, testCases);
        var formattedFeatureClassContent = FormatCode(featureClassContent);
        context.AddSource($"SourceGeneratedGherkinScenarios.g.cs", formattedFeatureClassContent);
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

    private static string GetTestClassContent(string assemblyName, IEnumerable<TestCase> testCases)
    {
        var featureClassContent =
           $$"""
            using BddDotNet;
            using Microsoft.Extensions.DependencyInjection;

            namespace {{assemblyName}};

            internal static partial class GherkinSourceGeneratorExtensions
            {
                public static partial void SourceGeneratedGherkinScenarios(this IServiceCollection services)
                {
                    {{GetTestCasesContent(testCases)}}
                }
            }
            """;

        return featureClassContent;
    }

    private static string GetTestCasesContent(IEnumerable<TestCase> testCases)
    {
        var declarations = new StringBuilder();
        foreach (var testCase in testCases)
        {
            declarations.AppendLine(GetTestCaseContent(testCase));
        }
        return declarations.ToString();
    }

    private static string GetTestCaseContent(TestCase testCase)
    {
        var declaration =
            $$""""
            services.Scenario(
                "{{testCase.AssemblyName}}",
                "{{testCase.AssemblyName}}",
                "{{testCase.Feature}}",
                "{{testCase.Scenario}}",
                """{{testCase.FeaturePath}}""",
                {{testCase.Line}},
                async context =>
                {
                    {{GetTestStepsContent(testCase)}}
                });
            """";

        return declaration;
    }

    private static string GetTestStepsContent(TestCase testCase)
    {
        var stepDeclarations = new StringBuilder();

        foreach (var step in testCase.Steps)
        {
            stepDeclarations.AppendLine(
                $""""
                #line ({step.Line}, {step.Column})-({step.Line + 1}, 1) 12 "{step.FeaturePath}"
                """");

            if (step.DataTable is { } dataTable)
            {
                stepDeclarations.AppendLine(
                    $$""""
                    await context.{{step.Keyword}}("""{{step.Text}}""", (object)new string[][]
                    {
                        {{GetDataTableContent(dataTable)}}
                    });
                    """");
            }
            else
            {
                stepDeclarations.AppendLine(
                    $""""
                    await context.{step.Keyword}("""{step.Text}""");
                    """");
            }
        }

        stepDeclarations.AppendLine("#line default");

        return stepDeclarations.ToString();
    }

    private static string GetDataTableContent(string[][] dataTable)
    {
        var dataTableDeclaration = new StringBuilder();

        foreach (var row in dataTable)
        {
            dataTableDeclaration.Append("new[] {");
            foreach (var cell in row)
            {
                dataTableDeclaration.Append(
                    $""""
                    """{cell}""",
                    """");
            }
            dataTableDeclaration.AppendLine("},");
        }

        return dataTableDeclaration.ToString();
    }
}
