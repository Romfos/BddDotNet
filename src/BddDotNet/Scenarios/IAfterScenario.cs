namespace BddDotNet.Scenarios;

public interface IAfterScenario
{
    Task AfterScenarioAsync(ScenarioContext context, Exception? exception);
}
