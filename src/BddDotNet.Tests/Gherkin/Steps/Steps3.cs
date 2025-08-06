using BddDotNet.Gherkin;

namespace BddDotNet.Tests.Gherkin.Steps;

internal sealed class Steps3(GherkinTraceService gherkinTraceService)
{
    [Given("this is given step with argument '(.*)'")]
    public void Step1(string argument)
    {
        gherkinTraceService.Trace(argument);
    }

    [Then("this is then step with table:")]
    public void Step2(string[][] actual)
    {
        gherkinTraceService.Trace(actual);
    }
}
