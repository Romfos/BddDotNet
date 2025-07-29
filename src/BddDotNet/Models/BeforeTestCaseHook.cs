namespace BddDotNet.Models;

internal sealed class BeforeTestCaseHook(Func<IServiceProvider, Task> method)
{
    public Func<IServiceProvider, Task> Method { get; } = method;
}
