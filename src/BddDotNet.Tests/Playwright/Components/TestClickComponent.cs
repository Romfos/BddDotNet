using BddDotNet.Playwright;
using BddDotNet.Playwright.Contracts;

namespace BddDotNet.Tests.Playwright.Components;

internal sealed class TestClickComponent(List<object?> traces) : IComponent, IClick
{
    public Task ClickAsync()
    {
        traces.Add("TestClickComponent");
        return Task.CompletedTask;
    }
}
