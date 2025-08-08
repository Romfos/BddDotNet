using BddDotNet.Components.Options;
using BddDotNet.Components.Routing;
using BddDotNet.Components.WebContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace BddDotNet.Playwright.DefaultComponents;

public sealed class Label([ServiceKey] string path, IOptionsService optionsService, IPage page) : IComponent, IGetValue<string?>, IVisible
{
    private readonly string locator = optionsService.GetOptions<string>(path);

    public async Task<string?> GetValueAsync()
    {
        var textContent = await page.Locator(locator).TextContentAsync();
        return textContent?.Trim();
    }

    public async Task<bool> IsVisibleAsync()
    {
        return await page.Locator(locator).IsVisibleAsync();
    }
}
