using BddDotNet.Gherkin.Models;

namespace BddDotNet.Gherkin.Services;

internal sealed class ScenarioContext(StepExecutionService stepExecutionService) : IScenarioContext
{
    public async Task Given(string text)
    {
        await stepExecutionService.ExecuteAsync(StepType.Given, text);
    }

    public async Task When(string text)
    {
        await stepExecutionService.ExecuteAsync(StepType.When, text);
    }

    public async Task Then(string text)
    {
        await stepExecutionService.ExecuteAsync(StepType.Then, text);
    }
}