namespace BddDotNet.Extensibility;

public interface IScenarioContext
{
    Task Given(string text);
    Task When(string text);
    Task Then(string text);
}
