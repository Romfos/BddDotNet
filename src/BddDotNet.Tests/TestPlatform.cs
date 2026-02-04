using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Builder;

namespace BddDotNet.Tests;

internal static class TestPlatform
{
    public static async Task<int> RunTestAsync(Action<IServiceCollection> configure)
    {
        var builder = await TestApplication.CreateBuilderAsync([]);
        configure(builder.AddBddDotNet());
        using var testApp = await builder.BuildAsync();
        return await testApp.RunAsync();
    }
}
