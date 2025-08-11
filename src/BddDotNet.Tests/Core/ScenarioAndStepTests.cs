namespace BddDotNet.Tests.Core;

[TestClass]
public sealed class ScenarioAndStepTests
{
    [TestMethod]
    public async Task GivenStepTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Given("given1");
                traces.Add(3);
            });

            services.Given(new("given1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task WhenStepTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.When("when1");
                traces.Add(3);
            });

            services.When(new("when1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }

    [TestMethod]
    public async Task ThenStepTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                traces.Add(1);
                await context.Then("then1");
                traces.Add(3);
            });

            services.Then(new("then1"), () =>
            {
                traces.Add(2);
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }
}
