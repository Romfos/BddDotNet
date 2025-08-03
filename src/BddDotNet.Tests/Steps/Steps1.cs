using BddDotNet.Gherkin;
using BddDotNet.Tests.Services;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps1(TraceService traceService)
{
    [Given("this is simple given step")]
    public void Step1()
    {
        traceService.Trace("this is simple given step");
    }

    [When("this is simple when step")]
    public void Step2()
    {
        traceService.Trace("this is simple when step");
    }

    [Then("this is simple then step")]
    public void Step3()
    {
        traceService.Trace("this is simple then step");
    }
}