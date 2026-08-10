using BddDotNet.Steps;

namespace BddDotNet.Internal.Models;

internal sealed record StepFallback(
    Func<StepFallbackContext, IServiceProvider, Task> Body);