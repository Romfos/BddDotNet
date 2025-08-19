using BddDotNet.Gherkin.SourceGenerator.Models;
using Gherkin;
using Gherkin.Ast;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;

namespace BddDotNet.Gherkin.SourceGenerator.Services;

internal sealed class GherkinParserService
{
    private readonly Parser gherkinParser = new();

    public GherkinParserResult Parse(string assemblyName, ImmutableArray<AdditionalText> featureFiles)
    {
        var result = new GherkinParserResult();

        foreach (var featureFile in featureFiles)
        {
            var featureDocument = GetFeatureDocument(result, featureFile);

            if (featureDocument != null)
            {
                ParseFeature(result, assemblyName, featureFile.Path, featureDocument);
            }
        }

        return result;
    }

    private Feature? GetFeatureDocument(GherkinParserResult result, AdditionalText featureFile)
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
                result.Errors.Add(new GherkinParserError(
                    error.Message,
                    featureFile.Path,
                    error.Location?.Line ?? 1,
                    error.Location?.Column ?? 1));
            }
        }

        return null;
    }

    public void ParseFeature(GherkinParserResult result, string assemblyName, string featureFilePath, Feature feature)
    {
        var featureBackground = GetGherkinBackground(result, feature, featureFilePath);

        foreach (var rule in feature.Children.OfType<Rule>())
        {
            var ruleBackground = GetGherkinBackground(result, rule, featureFilePath);

            foreach (var scenario in rule.Children.OfType<Scenario>())
            {
                var gherkinScenario = GetGherkinScenario(
                    assemblyName,
                    feature.Name,
                    featureFilePath,
                    scenario,
                    ruleBackground);

                if (scenario.Examples.Any())
                {
                    ParseScenarioOutline(result, gherkinScenario, scenario.Examples);
                }
                else
                {
                    result.Scenarios.Add(gherkinScenario);
                }
            }
        }

        foreach (var scenario in feature.Children.OfType<Scenario>())
        {
            var gherkinScenario = GetGherkinScenario(
                assemblyName,
                feature.Name,
                featureFilePath,
                scenario,
                featureBackground);

            if (scenario.Examples.Any())
            {
                ParseScenarioOutline(result, gherkinScenario, scenario.Examples);
            }
            else
            {
                result.Scenarios.Add(gherkinScenario);
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

    private GherkinBackground? GetGherkinBackground(GherkinParserResult result, IHasChildren container, string featureFilePath)
    {
        var backgrounds = container.Children.OfType<Background>().ToList();

        if (backgrounds.Count == 0)
        {
            return null;
        }

        if (backgrounds.Count > 1)
        {
            result.Errors.Add(new GherkinParserError(
                "Multiple backgrounds are not supported",
                featureFilePath,
                backgrounds[1].Location.Line,
                backgrounds[1].Location.Column));

            return null;
        }

        var gherkinBackground = new GherkinBackground(result.Backgrounds.Count);
        gherkinBackground.Steps.AddRange(GetGherkinSteps(featureFilePath, backgrounds[0]));

        result.Backgrounds.Add(gherkinBackground);

        return gherkinBackground;
    }

    private void ParseScenarioOutline(GherkinParserResult result, GherkinScenario originalScenario, IEnumerable<Examples> examples)
    {
        var index = 0;

        foreach (var example in examples)
        {
            var exampleHeaderCells = example.TableHeader.Cells.Select(x => x.Value).ToArray();

            foreach (var exampleTableRow in example.TableBody)
            {
                index++;

                var exampleValuesCells = exampleTableRow.Cells.Select(x => x.Value).ToArray();

                var scenario = originalScenario with
                {
                    Scenario = $"#{index}. {originalScenario.Scenario}",
                    Steps = GetScenarioOutlineSteps(originalScenario.Steps, exampleHeaderCells, exampleValuesCells).ToList()
                };

                result.Scenarios.Add(scenario);
            }
        }
    }

    private IEnumerable<GherkinStep> GetScenarioOutlineSteps(
        IEnumerable<GherkinStep> originalSteps,
        string[] exampleHeaderCells,
        string[] exampleValuesCells)
    {
        return originalSteps.Select(originalStep =>
        {
            return originalStep with
            {
                Text = Replace(originalStep.Text, exampleHeaderCells, exampleValuesCells),
                DataTable = originalStep.DataTable?
                   .Select(row => row.Select(cell => Replace(cell, exampleHeaderCells, exampleValuesCells)).ToArray())
                   .ToArray(),
            };
        });
    }

    private GherkinScenario GetGherkinScenario(
        string assemblyName,
        string featureName,
        string featureFilePath,
        Scenario scenario,
        GherkinBackground? background)
    {
        var gherkinScenario = new GherkinScenario(
            assemblyName,
            featureName,
            scenario.Name,
            featureFilePath,
            scenario.Location.Line,
            background);

        gherkinScenario.Steps.AddRange(GetGherkinSteps(featureFilePath, scenario));

        return gherkinScenario;
    }

    private IEnumerable<GherkinStep> GetGherkinSteps(string featureFilePath, IHasSteps container)
    {
        var keyword = "Given";

        foreach (var step in container.Steps)
        {
            if (step.KeywordType != StepKeywordType.Conjunction)
            {
                keyword = step.Keyword;
            }

            var gherkinStep = new GherkinStep(
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

            yield return gherkinStep;
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
