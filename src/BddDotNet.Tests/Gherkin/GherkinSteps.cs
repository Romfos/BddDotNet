namespace BddDotNet.Tests.Gherkin;

internal sealed class GherkinSteps(GherkinTraceService gherkinTraceService)
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

    [Given("this is async task given step")]
    public async Task Step4()
    {
        await Task.Yield();
        gherkinTraceService.Trace("this is async task given step");
    }

    [Given("this is async value task given step")]
    public async ValueTask Step5()
    {
        await Task.Yield();
        gherkinTraceService.Trace("this is async value task given step");
    }

    [Given("this is given step with argument '(.*)'")]
    public void Step6(string argument)
    {
        gherkinTraceService.Trace(argument);
    }

    [Then("this is then step with table:")]
    public void Step7(string[][] actual)
    {
        gherkinTraceService.Trace(actual);
    }

    [Then("this is then step with doc string:")]
    public void Step8(string docString)
    {
        gherkinTraceService.Trace(docString);
    }
}