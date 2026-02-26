namespace BddDotNet.Steps;

public interface IStepFallback
{
    Task StepFallbackAsync(StepFallbackContext context);
}
