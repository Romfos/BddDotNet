using BddDotNet.Gherkin;
using Microsoft.Playwright;

namespace BddDotNet.Playwright.DefaultComponents.Internal;

internal sealed class PlaywrightSteps(IPage page)
{
    [Given(@"navigate to '([^']*)'")]
    public async Task Navigate(string url)
    {
        await page.GotoAsync(url);
    }

    [Then(@"page url should be '([^']*)'")]
    public async Task PageUrlShouldBe(string url)
    {
        await page.WaitForURLAsync(url);
    }
}
