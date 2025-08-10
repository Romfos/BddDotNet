using BddDotNet.Components;
using BddDotNet.Playwright.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Components;

[TestClass]
public sealed class ComponentsTests
{
    [TestMethod]
    public async Task ClickTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.PlaywrightContracts();
            services.Component<TestClickComponent>("button1").Options("button1 options");

            services.Scenario<ComponentsTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.When("click on 'button1'");
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, "button1 options", 2]);
    }
}
