using BddDotNet.Gherkin;
using BddDotNet.Tests.Services;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps1(TraceService traceService)
{
    [Given("this is given step")]
    public void Step1()
    {
        traceService.Step1 = true;
    }

    [When("this is when step")]
    public void Step2()
    {
        traceService.Step2 = true;
    }

    [Then("this is then step")]
    public void Step3()
    {
        traceService.Step3 = true;
    }
}
