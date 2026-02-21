namespace BddDotNet.Extensibility;

public interface IAfterStep
{
    Task AfterStep(Exception? exception);
}