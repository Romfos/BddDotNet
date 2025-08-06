using BddDotNet.Gherkin;
using BddDotNet.Playwright.Contracts;

namespace BddDotNet.Playwright.Steps;

internal sealed class DefaultComponentSteps(IRouteService routeService)
{
    [When("click on '(.*)'")]
    public async Task ClickStep(string path)
    {
        await routeService.GetComponent<IClick>(path).ClickAsync();
    }
}
