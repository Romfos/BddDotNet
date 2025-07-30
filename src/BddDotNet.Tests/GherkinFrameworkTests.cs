using BddDotNet.Gherkin.Extensions;

namespace BddDotNet.Tests;

[TestClass]
public sealed class GherkinFrameworkTests
{
    [TestMethod]
    public async Task ScenarioTest()
    {
        var traces = new List<int>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<GherkinFrameworkTests>("feature1", "scenario1", async context =>
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
