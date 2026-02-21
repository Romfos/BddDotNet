using BddDotNet.Scenarios;
using BddDotNet.Steps;
using BddDotNet.Tests.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Steps;

[TestClass]
public sealed class AfterStepTests
{
    [TestMethod]
    public async Task AfterStepTest()
    {
        var traces = new List<string>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.AfterStep<AfterStep1>();

            services.Then(new("then1"), () => traces.Add("1"));
            services.Then(new("then2"), () => { throw new Exception("2"); });

            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("then1");
                await scenario.Then("then2");
            });
        });

        Assert.IsTrue(traces is ["1", "AfterStep then1 null", "AfterStep then2 2"]);
    }
}

file sealed class AfterStep1(List<string> traces) : IAfterStep
{
    public Task AfterStepAsync(StepContext context, Exception? exception)
    {
        traces.Add($"AfterStep {context.Text} {exception?.Message ?? "null"}");
        return Task.CompletedTask;
    }
}
