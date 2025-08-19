using BddDotNet.Gherkin.SourceGenerator.Exceptions;
using BddDotNet.Gherkin.SourceGenerator.Models;
using Gherkin;
using Gherkin.Ast;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace BddDotNet.Gherkin.SourceGenerator.Services;

internal sealed class TestCasesParser
{
    private readonly Parser gherkinParser = new();

    public List<TestCase> Parse(string assemblyName, ImmutableArray<AdditionalText> featureFiles)
    {
        var testCases = new List<TestCase>();
        var errors = new List<TestCasesParserError>();

        foreach (var featureFile in featureFiles)
        {
            try
            {
                var featureTestCases = GetFeatureTestCases(
                    assemblyName,
                    featureFile.Path,
                    featureFile.GetText()?.ToString()!);

                testCases.AddRange(featureTestCases);
            }
            catch (CompositeParserException exception)
            {
                foreach (var error in exception.Errors)
                {
                    errors.Add(new TestCasesParserError(
                        error.Message,
                        featureFile.Path,
                        error.Location?.Line ?? 1,
                        error.Location?.Column ?? 1));
                }
            }
        }

        if (errors.Any())
        {
            throw new TestCasesParserException(errors);
        }

        return testCases;
    }

    public IEnumerable<TestCase> GetFeatureTestCases(string assemblyName, string featureFilePath, string featureFileText)
    {
        using var stringReader = new StringReader(featureFileText);
        var feature = gherkinParser.Parse(stringReader).Feature;

        foreach (var rule in feature.Children.OfType<Rule>())
        {
            foreach (var scenario in rule.Children.OfType<Scenario>())
            {
                var testCase = GetTestCaseForScenario(
                    assemblyName,
                    feature.Name,
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

        foreach (var scenario in feature.Children.OfType<Scenario>())
        {
            var testCase = GetTestCaseForScenario(
                assemblyName,
                feature.Name,
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

        var unsupportedFeatureElementTypes = feature.Children
            .Where(x => x is not Scenario and not Rule)
            .Select(x => x.GetType().Name)
            .Distinct()
            .ToList();

        if (unsupportedFeatureElementTypes.Any())
        {
            throw new Exception($"Unsupported feature items: {string.Join(",", unsupportedFeatureElementTypes)}");
        }
    }

    private IEnumerable<TestCase> GetScenarioOutlineTestCases(TestCase originalTestCase, IEnumerable<Examples> examples)
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

    private IEnumerable<TestCaseStep> GetScenarioOutlineTestSteps(IEnumerable<TestCaseStep> originalSteps, string[] exampleHeaderCells, string[] exampleValuesCells)
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

    private TestCase GetTestCaseForScenario(string assemblyName, string featureName, string featureFilePath, Scenario scenario)
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

    private IEnumerable<TestCaseStep> GetScenarioTestSteps(string featureFilePath, Scenario scenario)
    {
        var keyword = "Given";

        foreach (var step in scenario.Steps)
        {
            if (step.KeywordType != StepKeywordType.Conjunction)
            {
                keyword = step.Keyword;
            }

            var testCaseStep = new TestCaseStep(
                keyword,
                step.Text,
                step.Argument is DataTable dataTable
                    ? dataTable.Rows.Select(x => x.Cells.Select(x => x.Value).ToArray()).ToArray()
                    : null,
                step.Argument is DocString docString
                    ? docString.Content
                    : null,
                featureFilePath,
                step.Location.Line,
                step.Location.Column);

            yield return testCaseStep;
        }
    }

    private string Replace(string text, string[] exampleHeaderCells, string[] exampleValuesCells)
    {
        for (var i = 0; i < exampleHeaderCells.Length; i++)
        {
            text = text.Replace($"<{exampleHeaderCells[i]}>", exampleValuesCells[i]);
        }
        return text;
    }
}
