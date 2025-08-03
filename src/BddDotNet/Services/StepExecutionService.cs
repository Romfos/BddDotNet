using BddDotNet.Exceptions;
using BddDotNet.Models;
using System.Text.RegularExpressions;

namespace BddDotNet.Services;

internal sealed class StepExecutionService(IServiceProvider serviceProvider, IEnumerable<Step> steps)
{
    public async Task ExecuteAsync(StepType stepType, string text, object?[] additionalStepArguments)
    {
        var (step, match) = FindGherkinStep(stepType, text);

        var handler = step.Factory(serviceProvider);
        var result = handler.DynamicInvoke([.. PrepareStepArguments(handler, match, additionalStepArguments)]);

        if (result is Task task)
        {
            await task;
        }

        if (result is ValueTask valueTask)
        {
            await valueTask;
        }
    }

    private IEnumerable<object?> PrepareStepArguments(Delegate handler, Match match, object?[] additionalStepArguments)
    {
        foreach (var parameter in handler.Method.GetParameters())
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
