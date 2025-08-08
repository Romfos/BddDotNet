using BddDotNet.Components.Routing;
using BddDotNet.Gherkin;

namespace BddDotNet.Components.Web.Internal;

internal sealed class WebContractsSteps(IRoutingService routingService)
{
    [When("click on '(.*)'")]
    public async Task ClickStep(string path)
    {
        await routingService.GetComponent<IClick>(path).ClickAsync();
    }

    [When(@"set following values:")]
    public async Task WhenSetFollowingValuesToFields(string[][] values)
    {
        foreach (var (path, value) in values.AsNameValueTable())
        {
            await routingService.GetComponent<ISetValue<string>>(path).SetValueAsync(value);
        }
    }

    [Then(@"should have following values:")]
    public async Task ThenFieldsShouldHaveFollowingValues(string[][] values)
    {
        var errors = new List<string>();

        foreach (var (path, expected) in values.AsNameValueTable())
        {
            var actual = await routingService.GetComponent<IGetValue<string>>(path).GetValueAsync();

            if ((expected, actual) is (null, not null) or (not null, null)
                || expected != null && !expected.Equals(actual))
            {
                errors.Add($"Path '{path}'. Actual '{actual}'. Expected: '{expected}'");
            }
        }

        if (errors.Any())
        {
            throw new Exception($"Some components have unexpected values: {string.Join(Environment.NewLine, errors)}");
        }
    }

    [Then(@"should be visible:")]
    public async Task ThenShouldBeVisible(string[][] values)
    {
        var errors = new List<string>();

        foreach (var path in values.AsSingleColumnNameTable())
        {
            if (!await routingService.GetComponent<IVisible>(path).IsVisibleAsync())
            {
                errors.Add(path);
            }
        }

        if (errors.Any())
        {
            throw new Exception($"Some components are invisible: {string.Join(",", errors)}");
        }
    }

    [Then(@"should be invisible:")]
    public async Task ThenShouldBeInvisible(string[][] values)
    {
        var errors = new List<string>();

        foreach (var path in values.AsSingleColumnNameTable())
        {
            if (await routingService.GetComponent<IVisible>(path).IsVisibleAsync())
            {
                errors.Add(path);
            }
        }

        if (errors.Any())
        {
            throw new Exception($"Some components are visible: {string.Join(",", errors)}");
        }
    }

    [Then(@"should be enabled:")]
    public async Task ThenShouldBeEnabled(string[][] values)
    {
        var errors = new List<string>();

        foreach (var path in values.AsSingleColumnNameTable())
        {
            if (await routingService.GetComponent<IEnabled>(path).IsEnabledAsync())
            {
                errors.Add(path);
            }
        }

        if (errors.Any())
        {
            throw new Exception($"Some components are disabled: {string.Join(",", errors)}");
        }
    }

    [Then(@"should be disabled:")]
    public async Task ThenShouldBeDisabled(string[][] values)
    {
        var errors = new List<string>();

        foreach (var path in values.AsSingleColumnNameTable())
        {
            if (!await routingService.GetComponent<IEnabled>(path).IsEnabledAsync())
            {
                errors.Add(path);
            }
        }

        if (errors.Any())
        {
            throw new Exception($"Some components are enabled: {string.Join(",", errors)}");
        }
    }
}
