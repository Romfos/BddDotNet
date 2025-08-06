using BddDotNet.Gherkin;

namespace BddDotNet.Tests.Gherkin.Steps;

internal sealed class Steps1(GherkinTraceService gherkinTraceService)
{
    [Given("this is simple given step")]
    public void Step1()
    {
        gherkinTraceService.Trace("this is simple given step");
    }

    [When("this is simple when step")]
    public void Step2()
    {
        gherkinTraceService.Trace("this is simple when step");
    }

    [Then("this is simple then step")]
    public void Step3()
    {
        gherkinTraceService.Trace("this is simple then step");
    }
}