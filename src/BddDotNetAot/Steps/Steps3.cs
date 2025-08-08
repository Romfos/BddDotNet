namespace BddDotNetAot.Steps;

internal sealed class Steps3
{
    [Given("this is given step with argument '(.*)'")]
    public void Step1(string actual)
    {
        if (actual is not ("abcd" or "sharpener" or "pencil"))
        {
            throw new Exception("Step4");
        }
    }

    [Then("this is then step with table:")]
    public void Step2(string[][] actual)
    {
        if (actual
            is not ([["book", "price"], ["sharpener", "30"], ["pencil", "15"]]
            or [["book", "price"], ["sharpener", "30"], ["static", "99"]]
            or [["book", "price"], ["pencil", "15"], ["static", "99"]]))
        {
            throw new Exception("Step5");
        }
    }
}
