using BddDotNet.Exceptions;
using BddDotNet.Extensibility;
using BddDotNet.Models;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BddDotNet.Services;

internal sealed class StepExecutionService(
    IServiceProvider serviceProvider,
    IEnumerable<Step> steps,
    IEnumerable<IArgumentTransformation> argumentTransformations)
{
    public async Task ExecuteAsync(StepType stepType, string text, object?[] additionalStepArguments)
    {
        var (step, match) = FindGherkinStep(stepType, text);

        var handler = step.Factory(serviceProvider);
        var arguments = await GetStepArgumentsAsync(handler, match, additionalStepArguments);
        var result = handler.DynamicInvoke(arguments);

        if (result is Task task)
        {
            await task;
        }

        if (result is ValueTask valueTask)
        {
            await valueTask;
        }
    }

    private async ValueTask<object?[]> GetStepArgumentsAsync(Delegate handler, Match match, object?[] additionalStepArguments)
    {
        var parameters = handler.Method.GetParameters();

        var arguments = ExtractStepArguments(parameters, match, additionalStepArguments).ToArray();

        for (var i = 0; i < parameters.Length; i++)
        {
            arguments[i] = await TransformAsync(arguments[i], parameters[i]);
        }

        return arguments;
    }

    private async ValueTask<object?> TransformAsync(object? argument, ParameterInfo parameter)
    {
        foreach (var argumentTransformation in argumentTransformations)
        {
            argument = await argumentTransformation.TransformAsync(argument, parameter.ParameterType);
        }

        return argument;
    }

    private IEnumerable<object?> ExtractStepArguments(ParameterInfo[] parameters, Match match, object?[] additionalStepArguments)
    {
        foreach (var parameter in parameters)
        {
            if (serviceProvider.GetService(parameter.ParameterType) is object service)
            {
                yield return service;
            }
            else
            {
                break;
            }
        }

        foreach (var group in match.Groups.Cast<Group>().Skip(1))
        {
            yield return group.Value;
        }

        foreach (var additionalArgument in additionalStepArguments)
        {
            yield return additionalArgument;
        }
    }

    private (Step, Match) FindGherkinStep(StepType stepType, string text)
    {
        var matchedSteps = steps
            .Where(step => step.StepType == stepType)
            .Select(step => (step, match: step.Pattern.Match(text)))
            .Where(x => x.match.Success)
            .Take(2)
            .ToArray();

        if (matchedSteps.Length == 1)
        {
            return matchedSteps[0];
        }

        if (matchedSteps.Length == 0)
        {
            throw new UnableToFindStepException(stepType, text);
        }

        throw new MultipleMatchedStepsFoundException(stepType, text);
    }
}
