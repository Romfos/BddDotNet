using BddDotNet.Extensions;
using BddDotNet.Gherkin.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Builder;

namespace BddDotNet.Tests;

[TestClass]
public sealed class BddDotNetGherkinTests
{
    private async Task<int> RunTestAsync(Action<IServiceCollection> configure)
    {
        var builder = await TestApplication.CreateBuilderAsync(["--results-directory ./TestResults"]);
        configure(builder.AddBddDotNet());
        using var testApp = await builder.BuildAsync();
        return await testApp.RunAsync();
    }

    [TestMethod]
    public async Task ScenarioTest()
    {
        var traces = new List<int>();

        await RunTestAsync(services =>
        {
            services.Scenario<BddDotNetGherkinTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);

                await context.Given("given1");
                await context.When("when1");
                await context.Then("then1");
            });

            services.Given(new("given1"), (IServiceProvider services) =>
            {
                traces.Add(2);
                return Task.CompletedTask;
            });

            services.When(new("when1"), (IServiceProvider services) =>
            {
                traces.Add(3);
                return Task.CompletedTask;
            });

            services.Then(new("then1"), (IServiceProvider services) =>
            {
                traces.Add(4);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1, 2, 3, 4]);
    }
}
