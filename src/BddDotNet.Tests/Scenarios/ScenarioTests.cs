using BddDotNet.Scenarios;
using BddDotNet.Steps;

namespace BddDotNet.Tests.Scenarios;

[TestClass]
public sealed class ScenarioTests
{
    [TestMethod]
    public async Task GivenStepTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                traces.Add(1);
                await scenario.Given("given1");
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
            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                traces.Add(1);
                await scenario.When("when1");
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
            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                traces.Add(1);
                await scenario.Then("then1");
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
