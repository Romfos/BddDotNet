using BddDotNet.Components.Routing;
using BddDotNet.Gherkin;

namespace BddDotNet.Components.Browser.Steps;

internal sealed class ContractSteps(IRoutingService routingService)
{
    [When("click on '(.*)'")]
    public async Task ClickStep(string path)
    {
        await routingService.GetComponent<IClick>(path).ClickAsync();
    }
}
