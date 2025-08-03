using BddDotNet.Gherkin;

namespace BddDotNetAot.Steps;

internal sealed class Steps2
{
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
