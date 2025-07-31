using BddDotNet.Gherkin;

namespace BddDotNetApp.Steps;

internal sealed class Steps1
{
    [Given("this is given step")]
    public Task Step1()
    {
        return Task.CompletedTask;
    }

    [When("this is when step")]
    public Task Step2()
    {
        return Task.CompletedTask;
    }

    [Then("this is then step")]
    public Task Step3()
    {
        return Task.CompletedTask;
    }
}
