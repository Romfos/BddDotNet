using BddDotNet.Gherkin;
using BddDotNet.Tests.Services;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps2(TraceService traceService)
{
    [Given("this is async task given step")]
    public async Task Step1()
    {
        await Task.Yield();
        traceService.Trace("this is async task given step");
    }

    [Given("this is async value task given step")]
    public async ValueTask Step2()
    {
        await Task.Yield();
        traceService.Trace("this is async value task given step");
    }
}
