using BddDotNet.Steps;

namespace BddDotNet.Internal.Models;

internal sealed class StepFallback(Func<StepFallbackContext, IServiceProvider, Task> body)
{
    public Func<StepFallbackContext, IServiceProvider, Task> Body { get; } = body;
}