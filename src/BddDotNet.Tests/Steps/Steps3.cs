using BddDotNet.Gherkin;
using BddDotNet.Tests.Services;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps3(TraceService traceService)
{
    [Given("this is given step with argument '(.*)'")]
    public void Step1(string argument)
    {
        traceService.Trace(argument);
    }

    [Then("this is then step with table:")]
    public void Step2(string[][] actual)
    {
        traceService.Trace(actual);
    }
}
