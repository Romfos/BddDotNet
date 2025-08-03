using BddDotNet.Gherkin;

namespace BddDotNetAot.Steps;

internal sealed class Steps1
{
    [Given("this is given step")]
    public void Step1()
    {
    }

    [When("this is when step")]
    public void Step2()
    {
    }

    [Then("this is then step")]
    public void Step3()
    {
    }

    [Given("given step with argument '(.*)'")]
    public void Step4(string actual)
    {
        if (actual is not "abcd")
        {
            throw new Exception("Step4");
        }
    }

    [Then("this is step with table:")]
    public void Step5(string[][] actual)
    {
        if (actual is not [["book", "price"], ["sharpener", "30"], ["pencil", "15"]])
        {
            throw new Exception("Step5");
        }
    }
}
