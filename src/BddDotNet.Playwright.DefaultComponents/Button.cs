using BddDotNet.Components.Browser;
using BddDotNet.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace BddDotNet.Playwright.DefaultComponents;

public sealed class Button([ServiceKey] string path, IOptionsService optionsService, IPage page) : IComponent, IClick
{
    private readonly string locator = optionsService.GetOptions<string>(path);

    public async Task ClickAsync()
    {
        await page.ClickAsync(locator);
    }
}
