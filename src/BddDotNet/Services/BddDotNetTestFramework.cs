using BddDotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestFramework;
using Microsoft.Testing.Platform.Messages;
using Microsoft.Testing.Platform.Requests;

namespace BddDotNet.Services;

internal sealed class BddDotNetTestFramework(IServiceProvider services) : ITestFramework, IDataProducer
{
    private readonly TestCase[] testCases = services.GetServices<TestCase>().ToArray();

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
            await RunTestCasesAsync(context.MessageBus, runTestExecutionRequest);
        }
        else if (context.Request is DiscoverTestExecutionRequest discoverTestExecutionRequest)
        {
            await DiscoverTestCasesAsync(context.MessageBus, discoverTestExecutionRequest);
        }
        else
        {
            throw new NotImplementedException($"Unsupported request type {context.Request.GetType().FullName}");
        }

        context.Complete();
    }

    private async Task RunTestCasesAsync(IMessageBus messageBus, RunTestExecutionRequest runTestExecutionRequest)
    {
        foreach (var testCase in testCases)
        {
            var testNode = CreateTestNode(testCase);

            if (runTestExecutionRequest.Filter is not TestNodeUidListFilter testNodeUidListFilter
                || testNodeUidListFilter.TestNodeUids.Contains(testNode.Uid))
            {
                var testResultProperty = await RunTestCaseAsync(testCase);
                testNode.Properties.Add(testResultProperty);
                await messageBus.PublishAsync(this, new TestNodeUpdateMessage(runTestExecutionRequest.Session.SessionUid, testNode));
            }
        }
    }

    private async Task<IProperty> RunTestCaseAsync(TestCase testCase)
    {
        try
        {
            await using var scope = services.CreateAsyncScope();
            var testCaseExecutionService = scope.ServiceProvider.GetRequiredService<TestCaseExecutionService>();
            await testCaseExecutionService.ExecuteAsync(testCase);
            return PassedTestNodeStateProperty.CachedInstance;
        }
        catch (Exception ex)
        {
            return new FailedTestNodeStateProperty(ex.GetBaseException());
        }
    }

    private async Task DiscoverTestCasesAsync(IMessageBus messageBus, DiscoverTestExecutionRequest discoverTestExecutionRequest)
    {
        foreach (var testCase in testCases)
        {
            var testNode = CreateTestNode(testCase);
            testNode.Properties.Add(DiscoveredTestNodeStateProperty.CachedInstance);
            await messageBus.PublishAsync(this, new TestNodeUpdateMessage(discoverTestExecutionRequest.Session.SessionUid, testNode));
        }
    }

    private TestNode CreateTestNode(TestCase testCase)
    {
        var testMethodIdentifierProperty = new TestMethodIdentifierProperty(
            testCase.AssemblyName,
            testCase.Namespace,
            testCase.TestCaseTypeName,
            testCase.TestCaseName,
            0,
            [],
            typeof(void).FullName!);

        var testFileLocationProperty = new TestFileLocationProperty(
            testCase.FilePath,
            new LinePositionSpan(new LinePosition(testCase.LineNumber, 0), new LinePosition(testCase.LineNumber, 0)));

        var testNode = new TestNode()
        {
            Uid = $"{testCase.TestCaseTypeName} -> {testCase.TestCaseName}",
            DisplayName = testCase.TestCaseName,
            Properties = new PropertyBag(testMethodIdentifierProperty, testFileLocationProperty)
        };

        return testNode;
    }
}
