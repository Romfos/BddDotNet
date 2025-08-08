using BddDotNet.Components.Options;
using BddDotNet.Components.Routing;
using BddDotNet.Components.Web.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace BddDotNet.Playwright.DefaultComponents;

public sealed class Button([ServiceKey] string path, IOptionsService optionsService, IPage page) : IComponent, IClick, IVisible, IEnabled
{
    private readonly string locator = optionsService.GetOptions<string>(path);

    public async Task ClickAsync()
    {
        await page.ClickAsync(locator);
    }

    public async Task<bool> IsVisibleAsync()
    {
        return await page.Locator(locator).IsVisibleAsync();
    }

    public async Task<bool> IsEnabledAsync()
    {
        return await page.Locator(locator).IsEnabledAsync();
    }
}
