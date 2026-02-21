namespace BddDotNet.Scenarios;

public interface IBeforeScenario
{
    Task BeforeScenarioAsync(ScenarioContext context);
}
