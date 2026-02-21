using BddDotNet.Scenarios;
using BddDotNet.Steps;
using BddDotNet.Tests.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Steps;

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
            services.BeforeStep<BeforeStep1>();

            services.Then(new("then1"), () => traces.Add("1"));
            services.Then(new("then2"), () => traces.Add("2"));

            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("then1");
                await scenario.Then("then2");
            });
        });

        Assert.IsTrue(traces is ["BeforeStep then1", "1", "BeforeStep then2", "2"]);
    }
}

file sealed class BeforeStep1(List<string> traces) : IBeforeStep
{
    public Task BeforeStepAsync(StepContext context)
    {
        traces.Add($"BeforeStep {context.Text}");
        return Task.CompletedTask;
    }
}