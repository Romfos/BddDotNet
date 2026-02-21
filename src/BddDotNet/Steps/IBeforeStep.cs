namespace BddDotNet.Steps;

public interface IBeforeStep
{
    Task BeforeStepAsync(StepContext context);
}
