using BddDotNet.Configuration;
using BddDotNet.Internal.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestFramework;
using Microsoft.Testing.Platform.Messages;
using Microsoft.Testing.Platform.Requests;

namespace BddDotNet.Internal.Services;

internal sealed class BddDotNetTestFramework(IServiceCollection serviceCollection) : ITestFramework, IDataProducer
{
    public string Uid { get; } = nameof(BddDotNetTestFramework);
    public string Version { get; } = "1.0.0";
    public string DisplayName { get; } = nameof(BddDotNetTestFramework);
    public string Description { get; } = "BddDotNet test framework";
    public Type[] DataTypesProduced { get; } = [typeof(TestNodeUpdateMessage)];

    public Task<bool> IsEnabledAsync()
    {
        return Task.FromResult(true);
    }

    public Task<CloseTestSessionResult> CloseTestSessionAsync(CloseTestSessionContext context)
    {
        return Task.FromResult(new CloseTestSessionResult
        {
            IsSuccess = true
        });
    }

    public Task<CreateTestSessionResult> CreateTestSessionAsync(CreateTestSessionContext context)
    {
        return Task.FromResult(new CreateTestSessionResult
        {
            IsSuccess = true
        });
    }

    public async Task ExecuteRequestAsync(ExecuteRequestContext context)
    {
        await using (var services = serviceCollection.BuildServiceProvider())
        {
            var configuration = services.GetRequiredService<BddDotNetConfiguration>();

            if (context.Request is RunTestExecutionRequest runTestExecutionRequest)
            {
                if (configuration.Parallel)
                {
                    await RunParallelAsync(services, context.MessageBus, runTestExecutionRequest, configuration);
                }
                else
                {
                    await RunOneByOneAsync(services, context.MessageBus, runTestExecutionRequest);
                }
            }
            else if (context.Request is DiscoverTestExecutionRequest discoverTestExecutionRequest)
            {
                await DiscoverTestNodesAsync(services, context.MessageBus, discoverTestExecutionRequest);
            }
            else
            {
                throw new NotImplementedException($"Unsupported request type {context.Request.GetType().FullName}");
            }
        }

        context.Complete();
    }

    private async Task RunParallelAsync(IServiceProvider serviceProvider, IMessageBus messageBus, RunTestExecutionRequest request, BddDotNetConfiguration configuration)
    {
        var tasks = new List<Task>(configuration.MaxDegreeOfParallelism);

        foreach (var (scenario, testNode) in GetTestCasesAsync(serviceProvider, request))
        {
            tasks.Add(RunScenarioAsync(serviceProvider, messageBus, request, testNode, scenario));

            if (tasks.Count == configuration.MaxDegreeOfParallelism)
            {
                tasks.Remove(await Task.WhenAny(tasks));
            }
        }

        await Task.WhenAll(tasks);
    }

    private async Task RunOneByOneAsync(IServiceProvider serviceProvider, IMessageBus messageBus, RunTestExecutionRequest request)
    {
        foreach (var (scenario, testNode) in GetTestCasesAsync(serviceProvider, request))
        {
            await RunScenarioAsync(serviceProvider, messageBus, request, testNode, scenario);
        }
    }

    private IEnumerable<(Scenario, TestNode)> GetTestCasesAsync(IServiceProvider serviceProvider, RunTestExecutionRequest request)
    {
        foreach (var scenario in serviceProvider.GetServices<Scenario>())
        {
            var testNode = CreateTestNode(scenario);

            if (request.Filter is not TestNodeUidListFilter testNodeUidListFilter
                || testNodeUidListFilter.TestNodeUids.Contains(testNode.Uid))
            {
                yield return (scenario, testNode);
            }
        }
    }

    private async Task RunScenarioAsync(IServiceProvider serviceProvider, IMessageBus messageBus, RunTestExecutionRequest request, TestNode testNode, Scenario scenario)
    {
        var startTime = DateTimeOffset.Now;
        var testResultProperty = await ExecuteScenarioAsync(serviceProvider, scenario);
        var stopTime = DateTimeOffset.Now;

        var timingProperty = new TimingProperty(new TimingInfo(startTime, stopTime, stopTime - startTime));
        testNode.Properties.Add(testResultProperty);
        testNode.Properties.Add(timingProperty);

        await messageBus.PublishAsync(this, new TestNodeUpdateMessage(request.Session.SessionUid, testNode));
    }

    private async Task<IProperty> ExecuteScenarioAsync(IServiceProvider serviceProvider, Scenario scenario)
    {
        try
        {
            await using var scope = serviceProvider.CreateAsyncScope();
            var scenarioExecutionService = scope.ServiceProvider.GetRequiredService<ScenarioExecutionService>();
            await scenarioExecutionService.ExecuteAsync(scenario);

            return PassedTestNodeStateProperty.CachedInstance;
        }
        catch (Exception ex)
        {
            return new FailedTestNodeStateProperty(ex.GetBaseException());
        }
    }

    private async Task DiscoverTestNodesAsync(IServiceProvider serviceProvider, IMessageBus messageBus, DiscoverTestExecutionRequest request)
    {
        foreach (var scenario in serviceProvider.GetServices<Scenario>())
        {
            var testNode = CreateTestNode(scenario);
            testNode.Properties.Add(DiscoveredTestNodeStateProperty.CachedInstance);
            await messageBus.PublishAsync(this, new TestNodeUpdateMessage(request.Session.SessionUid, testNode));
        }
    }

    private TestNode CreateTestNode(Scenario scenario)
    {
        var testMethodIdentifierProperty = new TestMethodIdentifierProperty(
            scenario.AssemblyName,
            scenario.Namespace,
            scenario.Feature,
            scenario.Name,
            0,
            [],
            typeof(void).FullName!);

        var testFileLocationProperty = new TestFileLocationProperty(
            scenario.FilePath,
            new LinePositionSpan(new LinePosition(scenario.LineNumber, 0), new LinePosition(scenario.LineNumber, 0)));

        var testNode = new TestNode()
        {
            Uid = $"{scenario.Feature} -> {scenario.Name}",
            DisplayName = scenario.Name,
            Properties = new PropertyBag(testMethodIdentifierProperty, testFileLocationProperty),
        };

        return testNode;
    }
}
