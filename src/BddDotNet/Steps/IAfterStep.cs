namespace BddDotNet.Steps;

public interface IAfterStep
{
    Task AfterStepAsync(StepContext context, Exception? exception);
}