namespace BddDotNet.Extensibility;

public interface IScenarioContext
{
    Task Given(string text, params object?[] additionalStepArguments);
    Task When(string text, params object?[] additionalStepArguments);
    Task Then(string text, params object?[] additionalStepArguments);
}
