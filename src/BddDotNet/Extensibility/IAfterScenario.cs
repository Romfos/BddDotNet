namespace BddDotNet.Extensibility;

public interface IAfterScenario
{
    Task AfterScenario(Exception? exception);
}
