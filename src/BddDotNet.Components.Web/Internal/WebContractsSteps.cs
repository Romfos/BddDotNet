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
        if (values is not [["Name", "Value"], ..])
        {
            throw new ArgumentException("Invalid table format. Name-Value table is expected");
        }

        foreach (var row in values.Skip(1))
        {
            var path = row[0];
            var value = row[1];
            await routingService.GetComponent<ISetValue<string>>(path).SetValueAsync(value);
        }
    }

    [Then(@"should have following values:")]
    public async Task ThenFieldsShouldHaveFollowingValues(string[][] values)
    {
        if (values is not [["Name", "Value"], ..])
        {
            throw new ArgumentException("Invalid table format. Name-Value table is expected");
        }

        var errors = new List<string>();

        foreach (var row in values.Skip(1))
        {
            var path = row[0];
            var expected = row[1];
            var actual = await routingService.GetComponent<IGetValue<string>>(path).GetValueAsync();

            if ((expected, actual) is (null, not null) or (not null, null)
                || expected != null && !expected.Equals(actual))
            {
                errors.Add($"Path '{path}'. Actual '{actual}'. Expected: '{expected}'");
            }
        }

        if (errors is not [])
        {
            throw new Exception($"Some components have unexpected values: {string.Join(Environment.NewLine, errors)}");
        }
    }

    [Then(@"should be visible:")]
    public async Task ThenShouldBeVisible(string[][] values)
    {
        if (values is not [["Name"], ..])
        {
            throw new ArgumentException("Invalid table format. Single column table with Name header is expected");
        }

        var errors = new List<string>();

        foreach (var row in values.Skip(1))
        {
            var path = row[0];
            if (!await routingService.GetComponent<IVisible>(path).IsVisibleAsync())
            {
                errors.Add(path);
            }
        }

        if (errors is not [])
        {
            throw new Exception($"Some components are invisible: {string.Join(",", errors)}");
        }
    }

    [Then(@"should be invisible:")]
    public async Task ThenShouldBeInvisible(string[][] values)
    {
        if (values is not [["Name"], ..])
        {
            throw new ArgumentException("Invalid table format. Single column table with Name header is expected");
        }

        var errors = new List<string>();

        foreach (var row in values.Skip(1))
        {
            var path = row[0];
            if (await routingService.GetComponent<IVisible>(path).IsVisibleAsync())
            {
                errors.Add(path);
            }
        }

        if (errors is not [])
        {
            throw new Exception($"Some components are visible: {string.Join(",", errors)}");
        }
    }

    [Then(@"should be enabled:")]
    public async Task ThenShouldBeEnabled(string[][] values)
    {
        if (values is not [["Name"], ..])
        {
            throw new ArgumentException("Invalid table format. Single column table with Name header is expected");
        }

        var errors = new List<string>();

        foreach (var row in values.Skip(1))
        {
            var path = row[0];
            if (await routingService.GetComponent<IEnabled>(path).IsEnabledAsync())
            {
                errors.Add(path);
            }
        }

        if (errors is not [])
        {
            throw new Exception($"Some components are disabled: {string.Join(",", errors)}");
        }
    }

    [Then(@"should be disabled:")]
    public async Task ThenShouldBeDisabled(string[][] values)
    {
        if (values is not [["Name"], ..])
        {
            throw new ArgumentException("Invalid table format. Single column table with Name header is expected");
        }

        var errors = new List<string>();

        foreach (var row in values.Skip(1))
        {
            var path = row[0];
            if (!await routingService.GetComponent<IEnabled>(path).IsEnabledAsync())
            {
                errors.Add(path);
            }
        }

        if (errors is not [])
        {
            throw new Exception($"Some components are enabled: {string.Join(",", errors)}");
        }
    }
}
