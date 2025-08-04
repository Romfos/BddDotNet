using BddDotNet.Gherkin;

namespace BddDotNetAot.Steps;

internal sealed class Steps4
{
    [Given("this is given step with table transformation:")]
    public void Step1(Dictionary<string, string> data)
    {
        if (data?.ToArray() is not [{ Key: "first", Value: "1" }, { Key: "second", Value: "2" }])
        {
            throw new Exception("Step1");
        }
    }
}
