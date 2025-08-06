using BddDotNet.Gherkin;

namespace BddDotNet.Tests.Gherkin.Steps;

internal sealed class Steps2(GherkinTraceService gherkinTraceService)
{
    [Given("this is async task given step")]
    public async Task Step1()
    {
        await Task.Yield();
        gherkinTraceService.Trace("this is async task given step");
    }

    [Given("this is async value task given step")]
    public async ValueTask Step2()
    {
        await Task.Yield();
        gherkinTraceService.Trace("this is async value task given step");
    }
}
