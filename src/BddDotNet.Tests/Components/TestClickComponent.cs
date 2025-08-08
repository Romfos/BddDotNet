using BddDotNet.Components.Options;
using BddDotNet.Components.Routing;
using BddDotNet.Components.WebContracts;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Components;

internal sealed class TestClickComponent([ServiceKey] string path, IOptionsService optionsService, List<object?> traces) : IComponent, IClick
{
    private readonly string locator = optionsService.GetOptions<string>(path);

    public Task ClickAsync()
    {
        traces.Add(locator);
        return Task.CompletedTask;
    }
}
