using BddDotNet.Configuration;
using BddDotNet.Scenarios;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace BddDotNet.Tests.Scenarios;

[TestClass]
public sealed class ParallelModeTests
{
    [TestMethod]
    public async Task OneByOneExecutionTest()
    {
        var traces = new ConcurrentStack<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);

            var taskCompletionSource = new TaskCompletionSource<bool>();

            services.Scenario<ParallelModeTests>("feature1", "scenario1", context =>
            {
                traces.Push(1);
                return Task.CompletedTask;
            });

            services.Scenario<ParallelModeTests>("feature1", "scenario2", context =>
            {
                traces.Push(2);
                return Task.CompletedTask;
            });

            services.Scenario<ParallelModeTests>("feature1", "scenario3", context =>
            {
                traces.Push(3);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces.ToArray() is [3, 2, 1]);
    }

    [TestMethod]
    public async Task ParallelTest()
    {
        var traces = new ConcurrentStack<object?>();

        await TestPlatform.RunTestAsync(services =>
        {
            services.AddSingleton(traces);
            services.Configuration(new() { Parallel = true, MaxDegreeOfParallelism = 3 });

            var taskCompletionSource = new TaskCompletionSource<bool>();

            services.Scenario<ParallelModeTests>("feature1", "scenario1", async context =>
            {
                await Task.Yield();
                traces.Push(1);
            });

            services.Scenario<ParallelModeTests>("feature1", "scenario2", async context =>
            {
                await taskCompletionSource.Task;
                traces.Push(2);
            });

            services.Scenario<ParallelModeTests>("feature1", "scenario3", context =>
            {
                traces.Push(3);
                taskCompletionSource.TrySetResult(true);
                return Task.CompletedTask;
            });
        });

        Assert.IsTrue(traces.ToArray() is [2, 3, 1]);
    }
}