using BddDotNet.Arguments;
using BddDotNet.Scenarios;
using BddDotNet.Steps;
using BddDotNet.Tests.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Arguments;

[TestClass]
public sealed class ArgumentTransformationTests
{
    [TestMethod]
    public async Task StepArgumentTransformationTest()
    {
        var traces = new List<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.ArgumentTransformation<ArgumentTransformation1>();

            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(2);
                traces.Add(value);
            });

            services.Scenario<ScenarioTests>("feature1", "scenario1", async scenario =>
            {
                await scenario.Then("then1 abcd");
            });
        });

        Assert.IsTrue(traces is [1, "abcd", 2, "System.String"]);
    }
}

file sealed class ArgumentTransformation1(List<object?> traces) : IArgumentTransformation
{
    public ValueTask<object?> TransformAsync(object? input, Type targetType)
    {
        traces.Add(1);
        traces.Add(input);
        return new(targetType.FullName);
    }
}
