using BddDotNetAot.Models;

namespace BddDotNetAot.Steps;

internal sealed class Steps5
{
    [When("this is given step with model transformation:")]
    public void Step1(ModelTransformation model)
    {
        if (model is not { first: 1, Second: "abcd", third: 3 })
        {
            throw new Exception("Step1");
        }
    }
}
