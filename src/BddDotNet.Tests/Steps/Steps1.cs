using BddDotNet.Gherkin;

namespace BddDotNet.Tests.Steps;

internal sealed class Steps1(List<int> traces)
{
    [Given("this is given step")]
    public void Step1()
    {
        traces.Add(1);
    }

    [When("this is when step")]
    public void Step2()
    {
        traces.Add(2);
    }

    [Then("this is then step")]
    public void Step3()
    {
        traces.Add(3);
    }
}
