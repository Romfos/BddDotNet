using BddDotNet.Gherkin.SourceGenerator.Models;
using Gherkin;
using Gherkin.Ast;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace BddDotNet.Gherkin.SourceGenerator.Services;

internal sealed class TestCasesParser
{
    private readonly Parser gherkinParser = new();

    public TestCaseParserResult Parse(string assemblyName, ImmutableArray<AdditionalText> featureFiles)
    {
        var result = new TestCaseParserResult();

        foreach (var featureFile in featureFiles)
        {
            var featureDocument = GetFeatureDocument(result, featureFile);

            if (featureDocument != null)
            {
                ParseFeatureTestCases(result, assemblyName, featureFile.Path, featureDocument);
            }
        }

        return result;
    }

    private Feature? GetFeatureDocument(TestCaseParserResult result, AdditionalText featureFile)
    {
        try
        {
            using var stringReader = new StringReader(featureFile.GetText()?.ToString()!);
            return gherkinParser.Parse(stringReader).Feature;
        }
        catch (CompositeParserException exception)
        {
            foreach (var error in exception.Errors)
            {
                result.Errors.Add(new TestCasesParserError(
                    error.Message,
                    featureFile.Path,
                    error.Location?.Line ?? 1,
                    error.Location?.Column ?? 1));
            }
        }

        return null;
    }

    public void ParseFeatureTestCases(TestCaseParserResult result, string assemblyName, string featureFilePath, Feature feature)
    {
        var featureBackground = GetTestCaseBackground(result, feature, featureFilePath);

        foreach (var rule in feature.Children.OfType<Rule>())
        {
            var ruleBackground = GetTestCaseBackground(result, rule, featureFilePath);

            foreach (var scenario in rule.Children.OfType<Scenario>())
            {
                var testCase = GetTestCaseForScenario(
                    assemblyName,
                    feature.Name,
                    featureFilePath,
                    scenario,
                    ruleBackground);

                if (scenario.Examples.Any())
                {
                    ParseScenarioOutlineTestCases(result, testCase, scenario.Examples);
                }
                else
                {
                    result.TestCases.Add(testCase);
                }
            }
        }

        foreach (var scenario in feature.Children.OfType<Scenario>())
        {
            var testCase = GetTestCaseForScenario(
                assemblyName,
                feature.Name,
                featureFilePath,
                scenario,
                featureBackground);

            if (scenario.Examples.Any())
            {
                ParseScenarioOutlineTestCases(result, testCase, scenario.Examples);
            }
            else
            {
                result.TestCases.Add(testCase);
            }
        }

        CheckUnsupportedFeatureElementTypes(feature);
    }

    private void CheckUnsupportedFeatureElementTypes(Feature feature)
    {
        var unsupportedFeatureElementTypes = feature.Children
            .Where(x => x is not Scenario and not Rule and not Background)
            .Concat(feature.Children.OfType<Rule>().SelectMany(x => x.Children).Where(x => x is not Scenario and not Background))
            .Select(x => x.GetType().Name)
            .Distinct()
            .ToList();

        if (unsupportedFeatureElementTypes.Any())
        {
            throw new Exception($"Unsupported feature items: {string.Join(",", unsupportedFeatureElementTypes)}");
        }
    }

    private TestCaseBackground? GetTestCaseBackground(TestCaseParserResult result, IHasChildren container, string featureFilePath)
    {
        var backgrounds = container.Children.OfType<Background>().ToList();

        if (backgrounds.Count == 0)
        {
            return null;
        }

        if (backgrounds.Count > 1)
        {
            result.Errors.Add(new TestCasesParserError(
                "Multiple backgrounds are not supported",
                featureFilePath,
                backgrounds[1].Location.Line,
                backgrounds[1].Location.Column));

            return null;
        }

        var testCaseBackground = new TestCaseBackground(result.Backgrounds.Count);
        testCaseBackground.Steps.AddRange(GetTestSteps(featureFilePath, backgrounds[0]));

        result.Backgrounds.Add(testCaseBackground);

        return testCaseBackground;
    }

    private void ParseScenarioOutlineTestCases(TestCaseParserResult result, TestCase originalTestCase, IEnumerable<Examples> examples)
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

                result.TestCases.Add(testCase);
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

    private TestCase GetTestCaseForScenario(string assemblyName, string featureName, string featureFilePath, Scenario scenario, TestCaseBackground? background)
    {
        var testCase = new TestCase(
            assemblyName,
            featureName,
            scenario.Name,
            featureFilePath,
            scenario.Location.Line,
            background);

        testCase.Steps.AddRange(GetTestSteps(featureFilePath, scenario));

        return testCase;
    }

    private IEnumerable<TestCaseStep> GetTestSteps(string featureFilePath, IHasSteps container)
    {
        var keyword = "Given";

        foreach (var step in container.Steps)
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
