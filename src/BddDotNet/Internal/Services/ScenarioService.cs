using BddDotNet.Scenarios;
using BddDotNet.Steps;

namespace BddDotNet.Internal.Services;

internal sealed class ScenarioService(StepExecutionService stepExecutionService) : IScenarioService
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