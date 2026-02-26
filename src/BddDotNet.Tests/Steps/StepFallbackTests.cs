using BddDotNet.Scenarios;
using BddDotNet.Steps;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Steps;

[TestClass]
public sealed class StepFallbackTests
{
    [TestMethod]
    public async Task StepFallbackTest()
    {
        var traces = new List<string>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.Fallback<StepFallback1>();

            services.Scenario("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("then1");
            });
        });

        Assert.IsTrue(traces is ["StepFallback Then then1"]);
    }
}

file sealed class StepFallback1(List<string> traces) : IStepFallback
{
    public Task StepFallbackAsync(StepFallbackContext context)
    {
        traces.Add($"StepFallback {context.Type} {context.Text}");
        return Task.CompletedTask;
    }
}