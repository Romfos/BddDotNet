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
        if (context.Request is RunTestExecutionRequest runTestExecutionRequest)
        {
            await RunTestExecutionAsync(context.MessageBus, runTestExecutionRequest);
        }
        else if (context.Request is DiscoverTestExecutionRequest discoverTestExecutionRequest)
        {
            await DiscoverTestExecutionAsync(context.MessageBus, discoverTestExecutionRequest);
        }
        else
        {
            throw new NotImplementedException($"Unsupported request type {context.Request.GetType().FullName}");
        }

        context.Complete();
    }

    private async Task RunTestExecutionAsync(IMessageBus messageBus, RunTestExecutionRequest request)
    {
        await using var services = serviceCollection.BuildServiceProvider();
        var configuration = services.GetRequiredService<BddDotNetConfiguration>();

        await foreach (var testNode in RunScenariosAsync(services, request, configuration))
        {
            await messageBus.PublishAsync(this, new TestNodeUpdateMessage(request.Session.SessionUid, testNode));
        }
    }

    private async Task DiscoverTestExecutionAsync(IMessageBus messageBus, DiscoverTestExecutionRequest request)
    {
        await using var services = serviceCollection.BuildServiceProvider();

        foreach (var scenario in services.GetServices<Scenario>())
        {
            var testNode = CreateTestNode(scenario);
            testNode.Properties.Add(DiscoveredTestNodeStateProperty.CachedInstance);
            await messageBus.PublishAsync(this, new TestNodeUpdateMessage(request.Session.SessionUid, testNode));
        }
    }

    private IAsyncEnumerable<TestNode> RunScenariosAsync(IServiceProvider serviceProvider, RunTestExecutionRequest request, BddDotNetConfiguration configuration)
    {
        if (configuration.MaxConcurrentTasks > 1)
        {
            return RunConcurrentAsync(serviceProvider, request, configuration);
        }
        else
        {
            return RunOneByOneAsync(serviceProvider, request);
        }
    }

    private async IAsyncEnumerable<TestNode> RunConcurrentAsync(IServiceProvider serviceProvider, RunTestExecutionRequest request, BddDotNetConfiguration configuration)
    {
        var tasks = new List<Task<TestNode>>(configuration.MaxConcurrentTasks);

        foreach (var scenario in GetRelevantScenarios(serviceProvider, request))
        {
            tasks.Add(RunScenarioAsync(serviceProvider, scenario));

            if (tasks.Count == configuration.MaxConcurrentTasks)
            {
                var task = await Task.WhenAny(tasks);
                tasks.Remove(task);
                yield return await task;
            }
        }

        foreach (var testNode in await Task.WhenAll(tasks))
        {
            yield return testNode;
        }
    }

    private async IAsyncEnumerable<TestNode> RunOneByOneAsync(IServiceProvider serviceProvider, RunTestExecutionRequest request)
    {
        foreach (var scenario in GetRelevantScenarios(serviceProvider, request))
        {
            yield return await RunScenarioAsync(serviceProvider, scenario);
        }
    }

    private async Task<TestNode> RunScenarioAsync(IServiceProvider serviceProvider, Scenario scenario)
    {
        var testNode = CreateTestNode(scenario);

        var startTime = DateTimeOffset.Now;
        var testResultProperty = await ExecuteScenarioAsync(serviceProvider, scenario);
        var stopTime = DateTimeOffset.Now;

        var timingProperty = new TimingProperty(new TimingInfo(startTime, stopTime, stopTime - startTime));
        testNode.Properties.Add(testResultProperty);
        testNode.Properties.Add(timingProperty);

        return testNode;
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

    private IEnumerable<Scenario> GetRelevantScenarios(IServiceProvider serviceProvider, RunTestExecutionRequest request)
    {
        foreach (var scenario in serviceProvider.GetServices<Scenario>())
        {
            var testNodeUid = GetTestNodeUid(scenario);

            if (request.Filter is not TestNodeUidListFilter testNodeUidListFilter
                || testNodeUidListFilter.TestNodeUids.Contains(testNodeUid))
            {
                yield return scenario;
            }
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
            Uid = GetTestNodeUid(scenario),
            DisplayName = scenario.Name,
            Properties = new PropertyBag(testMethodIdentifierProperty, testFileLocationProperty),
        };

        return testNode;
    }

    private TestNodeUid GetTestNodeUid(Scenario scenario)
    {
        return new($"{scenario.Feature} -> {scenario.Name}");
    }
}
