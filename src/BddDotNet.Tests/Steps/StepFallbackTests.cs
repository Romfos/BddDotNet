using BddDotNet.Scenarios;
using BddDotNet.Steps;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Steps;

[TestClass]
public sealed class StepFallbackTests
{
    [TestMethod]
    public async Task StepFallbackForDelegateTest()
    {
        var traces = new List<string>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            services.Fallback(async (context, services) =>
            {
                var traces = services.GetRequiredService<List<string>>();
                traces.Add($"Fallback {context.Type} {context.Text}");
            });

            services.Scenario("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("then1");
            });
        });

        Assert.IsTrue(traces is ["Fallback Then then1"]);
    }
}