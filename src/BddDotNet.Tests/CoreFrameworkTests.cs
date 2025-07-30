using BddDotNet.Extensions;

namespace BddDotNet.Tests;

[TestClass]
public sealed class CoreFrameworkTests
{
    [TestMethod]
    public async Task SingeTestCaseExecutionTest()
    {
        var traces = new List<int>();

        await Platform.RunTestAsync(services =>
        {
            services.TestCase<CoreFrameworkTests>("testCase1", services =>
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

        await Platform.RunTestAsync(services =>
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

            services.TestCase<CoreFrameworkTests>("testCase1", services =>
            {
                traces.Add(3);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces is [1, 2, 3]);
    }
}
