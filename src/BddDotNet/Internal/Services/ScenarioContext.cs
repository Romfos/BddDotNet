using BddDotNet.Extensibility;
using BddDotNet.Internal.Models;

namespace BddDotNet.Internal.Services;

internal sealed class ScenarioContext(StepExecutionService stepExecutionService) : IScenarioContext
{
    public async Task Given(string text, params object?[] additionalStepArguments)
    {
        await stepExecutionService.ExecuteAsync(StepType.Given, text, additionalStepArguments);
    }

    public async Task When(string text, params object?[] additionalStepArguments)
    {
        await stepExecutionService.ExecuteAsync(StepType.When, text, additionalStepArguments);
    }

    public async Task Then(string text, params object?[] additionalStepArguments)
    {
        await stepExecutionService.ExecuteAsync(StepType.Then, text, additionalStepArguments);
    }
}