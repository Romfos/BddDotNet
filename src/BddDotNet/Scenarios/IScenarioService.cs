namespace BddDotNet.Scenarios;

public interface IScenarioService
{
    Task Given(string text, params object?[] additionalStepArguments);
    Task When(string text, params object?[] additionalStepArguments);
    Task Then(string text, params object?[] additionalStepArguments);
}
