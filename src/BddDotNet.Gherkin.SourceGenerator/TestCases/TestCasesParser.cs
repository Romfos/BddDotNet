using Gherkin;
using Gherkin.Ast;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace BddDotNet.Gherkin.SourceGenerator.TestCases;

internal static class TestCasesParser
{
    private static readonly Parser gherkinParser = new();

    public static IEnumerable<TestCase> GetTestCases(string assemblyName, ImmutableArray<AdditionalText> featureFiles)
    {
        return featureFiles.SelectMany(x => GetTestCases(assemblyName, x.Path, x.GetText()?.ToString()!));
    }

    public static IEnumerable<TestCase> GetTestCases(string assemblyName, string featureFilePath, string featureFileText)
    {
        using var stringReader = new StringReader(featureFileText);
        var gherkinDocument = gherkinParser.Parse(stringReader);

        foreach (var scenario in gherkinDocument.Feature.Children.OfType<Scenario>())
        {
            var testCase = GetTestCaseForScenario(
                assemblyName,
                gherkinDocument.Feature.Name,
                featureFilePath,
                scenario);

            if (scenario.Examples.Any())
            {
                foreach (var outlineTestCase in GetScenarioOutlineTestCases(testCase, scenario.Examples))
                {
                    yield return outlineTestCase;
                }
            }
            else
            {
                yield return testCase;
            }
        }
    }

    private static IEnumerable<TestCase> GetScenarioOutlineTestCases(TestCase originalTestCase, IEnumerable<Examples> examples)
    {
        var index = 0;

        foreach (var example in examples)
        {
            var exampleHeaderCells = example.TableHeader.Cells.Select(x => x.Value).ToArray();

            foreach (var exampleTableRow in example.TableBody)
            {
                index++;

                var exampleValuesCells = exampleTableRow.Cells.Select(x => x.Value).ToArray();

                var testCase = originalTestCase with
                {
                    Scenario = $"#{index}. {originalTestCase.Scenario}",
                    Steps = GetScenarioOutlineTestSteps(originalTestCase.Steps, exampleHeaderCells, exampleValuesCells).ToList()
                };

                yield return testCase;
            }
        }
    }

    private static IEnumerable<TestCaseStep> GetScenarioOutlineTestSteps(IEnumerable<TestCaseStep> originalSteps, string[] exampleHeaderCells, string[] exampleValuesCells)
    {
        return originalSteps.Select(originalStep =>
        {
            return originalStep with
            {
                Text = Replace(originalStep.Text, exampleHeaderCells, exampleValuesCells),
                DataTable = originalStep.DataTable?
                   .Select(row => row.Select(cell => Replace(cell, exampleHeaderCells, exampleValuesCells)).ToArray())
                   .ToArray()
            };
        });
    }

    private static TestCase GetTestCaseForScenario(string assemblyName, string featureName, string featureFilePath, Scenario scenario)
    {
        var testCase = new TestCase(
            assemblyName,
            featureName,
            scenario.Name,
            featureFilePath,
            scenario.Location.Line);

        testCase.Steps.AddRange(GetScenarioTestSteps(featureFilePath, scenario));

        return testCase;
    }

    private static IEnumerable<TestCaseStep> GetScenarioTestSteps(string featureFilePath, Scenario scenario)
    {
        var keyword = "Given";

        foreach (var step in scenario.Steps)
        {
            if (!step.Keyword.StartsWith("And"))
            {
                keyword = step.Keyword;
            }

            var testCaseStep = new TestCaseStep(
                keyword,
                step.Text,
                step.Argument is DataTable dataTable
                    ? dataTable.Rows.Select(x => x.Cells.Select(x => x.Value).ToArray()).ToArray()
                    : null,
                featureFilePath,
                step.Location.Line,
                step.Location.Column);

            yield return testCaseStep;
        }
    }

    private static string Replace(string text, string[] exampleHeaderCells, string[] exampleValuesCells)
    {
        for (var i = 0; i < exampleHeaderCells.Length; i++)
        {
            text = text.Replace($"<{exampleHeaderCells[i]}>", exampleValuesCells[i]);
        }
        return text;
    }
}
