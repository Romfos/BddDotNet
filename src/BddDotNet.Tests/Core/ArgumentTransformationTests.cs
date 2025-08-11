using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Tests.Core;

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

            services.Scenario<ScenarioAndStepTests>("feature1", "scenario1", async context =>
            {
                await context.Then("then1 abcd");
            });
            services.Then(new("then1 (.*)"), (string value) =>
            {
                traces.Add(2);
                traces.Add(value);
            });

            services.ArgumentTransformation<ArgumentTransformation1>();
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
