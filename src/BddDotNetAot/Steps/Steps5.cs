using BddDotNetAot.Models;

namespace BddDotNetAot.Steps;

internal sealed class Steps5
{
    [When("this is given step with model transformation:")]
    public void Step1(Model1 model)
    {
        if (model is not { First: 1, Second: "abcd" })
        {
            throw new Exception("Step1");
        }
    }
}
