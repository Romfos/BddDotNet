using BddDotNet.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BddDotNet.Tests;

[TestClass]
public sealed class BddDotNetTests
{
    private async Task<int> RunTestAsync(Action<IServiceCollection> configure)
    {
        var builder = await TestApplication.CreateBuilderAsync(["--results-directory ./TestResults"]);
        configure(builder.AddBddDotNet());
        using var testApp = await builder.BuildAsync();
        return await testApp.RunAsync();
    }

    [TestMethod]
    public async Task SingeTestCaseExecutionTest()
    {
        var traces = new List<int>();

        await RunTestAsync(services =>
        {
            services.TestCase<BddDotNetTests>("testCase1", services =>
            {
                traces.Add(1);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1]);
    }

    [TestMethod]
    public async Task BeforeTestCaseHooksTest()
    {
        var traces = new List<int>();

        await RunTestAsync(services =>
        {
            services.BeforeTestCase(services =>
            {
                traces.Add(1);
                return Task.CompletedTask;
            });

            services.BeforeTestCase(services =>
            {
                traces.Add(2);
                return Task.CompletedTask;
            });

            services.TestCase<BddDotNetTests>("testCase1", services =>
            {
                traces.Add(3);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }
}
