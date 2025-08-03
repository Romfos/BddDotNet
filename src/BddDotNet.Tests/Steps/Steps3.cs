using BddDotNet.Gherkin;
using BddDotNet.Tests.Services;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps3(TraceService traceService)
{
    [Given("this is the first step with And keyword")]
    public void Step6()
    {
        traceService.Step6 = true;
    }

    [When("this is the second when step")]
    public void Step7()
    {
        traceService.Step7 = true;
    }

    [When("his is the third when step")]
    public void Step8()
    {
        traceService.Step8 = true;
    }
}
