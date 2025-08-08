using BddDotNet.Components.Options;
using BddDotNet.Components.Routing;
using BddDotNet.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;

namespace BddDotNet.Playwright.DefaultComponents;

public sealed class Input([ServiceKey] string path, IOptionsService optionsService, IPage page)
    : IComponent, ISetValue<string>, IGetValue<string>, IVisible, IEnabled
{
    private readonly string locator = optionsService.GetOptions<string>(path);

    public async Task<string> GetValueAsync()
    {
        return await page.Locator(locator).InputValueAsync();
    }

    public async Task SetValueAsync(string value)
    {
        await page.Locator(locator).FillAsync(value);
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
