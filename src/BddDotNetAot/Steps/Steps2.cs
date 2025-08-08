namespace BddDotNetAot.Steps;

internal sealed class Steps2
{
    [Given("this is async task given step")]
    public async Task Step1()
    {
        await Task.Yield();
    }

    [Given("this is async value task given step")]
    public async ValueTask Step2()
    {
        await Task.Yield();
    }
}
