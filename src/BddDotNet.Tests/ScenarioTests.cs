namespace BddDotNet.Tests;

[TestClass]
public sealed class ScenarioTests
{
    [TestMethod]
    public async Task GivenStepTest()
    {
        var traces = new List<int>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Given("given1");
                traces.Add(3);
            });

            services.Given(new("given1"), (IServiceProvider services) =>
            {
                traces.Add(2);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task WhenStepTest()
    {
        var traces = new List<int>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.When("when1");
                traces.Add(3);
            });

            services.When(new("when1"), (IServiceProvider services) =>
            {
                traces.Add(2);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task ThenStepTest()
    {
        var traces = new List<int>();

        await Platform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(3);
            });

            services.Then(new("then1"), (IServiceProvider services) =>
            {
                traces.Add(2);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }
}
