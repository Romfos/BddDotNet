using BddDotNet.Gherkin;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps1(List<int> traces)
{
    [Given("this is given step")]
    public Task Step1()
    {
        traces.Add(1);
        return Task.CompletedTask;
    }

    [When("this is when step")]
    public Task Step2()
    {
        traces.Add(2);
        return Task.CompletedTask;
    }

    [Then("this is then step")]
    public Task Step3()
    {
        traces.Add(3);
        return Task.CompletedTask;
    }
}
