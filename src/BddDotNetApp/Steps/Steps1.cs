using BddDotNet.Gherkin;

namespace BddDotNetApp.Steps;

internal sealed class Steps1
{
    [Given("this is given step")]
    public void Step1()
    {
    }

    [Given("given step with argument '(.*)'")]
    public void Step4(string argument)
    {
        if (argument != "1")
        {
            throw new Exception($"Unsupported argument {argument}");
        }
    }

    [When("this is when step")]
    public void Step2()
    {
    }

    [Then("this is then step")]
    public void Step3()
    {
    }
}
