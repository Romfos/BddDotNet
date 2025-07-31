using BddDotNet.Exceptions;
using BddDotNet.Models;

namespace BddDotNet.Services;

internal sealed class StepExecutionService(IServiceProvider serviceProvider, IEnumerable<Step> steps)
{
    public async Task ExecuteAsync(StepType stepType, string text)
    {
        var gherkinStep = FindGherkinStep(stepType, text);

        var result = gherkinStep.Body.DynamicInvoke(serviceProvider);

        if (result is Task task)
        {
            await task;
        }

        if (result is ValueTask valueTask)
        {
            await valueTask;
        }
    }

    private Step FindGherkinStep(StepType stepType, string text)
    {
        var matchedSteps = steps
            .Where(x => x.StepType == stepType && x.Pattern.IsMatch(text))
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
