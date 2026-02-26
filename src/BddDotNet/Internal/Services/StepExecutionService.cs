using BddDotNet.Arguments;
using BddDotNet.Internal.Exceptions;
using BddDotNet.Internal.Models;
using BddDotNet.Steps;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace BddDotNet.Internal.Services;

internal sealed class StepExecutionService(
    IServiceProvider serviceProvider,
    IEnumerable<Step> steps,
    IEnumerable<IArgumentTransformation> argumentTransformations,
    IEnumerable<IBeforeStep> beforeStepHooks,
    IEnumerable<IAfterStep> afterStepHook)
{
    public async Task ExecuteAsync(StepType stepType, string text, object?[] additionalStepArguments)
    {
        if (!FindGherkinStep(stepType, text, out var step, out var match))
        {
            if (serviceProvider.GetService<StepFallback>() is not StepFallback stepsFallback)
            {
                throw new UnableToFindStepException(stepType, text);
            }

            await stepsFallback.Body(new(stepType, text, additionalStepArguments), serviceProvider);

            return;
        }

        var handler = step.HandlerFactory(serviceProvider);
        var arguments = await GetStepArgumentsAsync(handler, match, additionalStepArguments);
        var stepContext = new StepContext(stepType, text, step.Pattern, arguments);

        await BeforeStep(stepContext);

        try
        {
            await ExecuteStepAsync(handler, arguments);
        }
        catch (Exception exception)
        {
            await AfterStep(stepContext, exception.GetBaseException());
            throw;
        }

        await AfterStep(stepContext, null);
    }

    private async Task BeforeStep(StepContext stepContext)
    {
        foreach (var beforeStepHook in beforeStepHooks)
        {
            await beforeStepHook.BeforeStepAsync(stepContext);
        }
    }

    private async Task AfterStep(StepContext stepContext, Exception? exception)
    {
        foreach (var afterStepHook in afterStepHook.Reverse())
        {
            await afterStepHook.AfterStepAsync(stepContext, exception);
        }
    }

    private async Task ExecuteStepAsync(Delegate handler, object?[] arguments)
    {
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

    private bool FindGherkinStep(StepType stepType, string text, [NotNullWhen(true)] out Step? step, [NotNullWhen(true)] out Match? match)
    {
        var matchedSteps = steps
            .Where(step => step.StepType == stepType)
            .Select(step => (step, match: step.Pattern.Match(text)))
            .Where(x => x.match.Success)
            .Take(2)
            .ToArray();

        if (matchedSteps.Length == 1)
        {
            step = matchedSteps[0].step;
            match = matchedSteps[0].match;
            return true;
        }

        if (matchedSteps.Length == 0)
        {
            step = null;
            match = null;
            return false;
        }

        throw new MultipleMatchedStepsFoundException(stepType, text);
    }
}
