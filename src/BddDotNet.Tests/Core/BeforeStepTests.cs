using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Core;

[TestClass]
public sealed class BeforeStepTests
{
    [TestMethod]
    public async Task BeforeStepTest()
    {
        var traces = new List<string>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.Then("then1");
            });

            services.Then(new("then1"), () =>
            {
                traces.Add("1");
            });

            services.BeforeStep<BeforeStep1>();
        });

        Assert.IsTrue(traces is ["BeforeStep", "1"]);
    }
}

file sealed class BeforeStep1(List<string> traces) : IBeforeStep
{
    public Task BeforeStep()
    {
        traces.Add("BeforeStep");
        return Task.CompletedTask;
    }
}