using BddDotNet.Models;

namespace BddDotNet.Services;

internal sealed class TestCaseExecutionService(
    IServiceProvider services,
    IEnumerable<BeforeTestCaseHook> beforeTestCaseHooks)
{
    public async Task ExecuteAsync(TestCase testCase)
    {
        foreach (var beforeTestCaseHook in beforeTestCaseHooks)
        {
            await beforeTestCaseHook.Method(services);
        }

        await testCase.Method(services);
    }
}
