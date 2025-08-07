using BddDotNet.Components.Browser;
using BddDotNet.Components.Routing;
using BddDotNet.Tests.Playwright.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Playwright;

[TestClass]
public sealed class PlaywrightTests
{
    [TestMethod]
    public async Task ClickContractTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.ComponentRoutingSystem();
            services.EnableBrowserContracts();

            services.Component<TestClickComponent>("main page -> login");
            services.ComponentOptions("main page -> login", ".btn-primary");

            services.Scenario<PlaywrightTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.When("click on 'main page -> login'");
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, "TestClickComponent", 2]);
    }
}
