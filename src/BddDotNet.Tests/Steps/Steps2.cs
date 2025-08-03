using BddDotNet.Gherkin;
using BddDotNet.Tests.Services;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps2(TraceService traceService)
{
    [Given("given step with argument '(.*)'")]
    public void Step4(string argument)
    {
        traceService.Step4 = argument;
    }

    [Then("this is step with table:")]
    public void Step5(string[][] actual)
    {
        traceService.Step5 = actual;
    }
}
