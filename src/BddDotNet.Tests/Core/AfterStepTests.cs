using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Core;

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

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.Then("then1");
                await context.Then("then2");
            });

            services.Then(new("then1"), () =>
            {
                traces.Add("1");
            });

            services.Then(new("then2"), () =>
            {
                throw new Exception("2");
            });

            services.AfterStep<AfterStep1>();
        });

        Assert.IsTrue(traces is ["1", "AfterStep null", "AfterStep 2"]);
    }
}

file sealed class AfterStep1(List<string> traces) : IAfterStep
{
    public Task AfterStep(Exception? exception)
    {
        traces.Add($"AfterStep {exception?.Message ?? "null"}");
        return Task.CompletedTask;
    }
}
