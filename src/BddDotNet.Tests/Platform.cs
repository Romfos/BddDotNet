using BddDotNet.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Builder;

namespace BddDotNet.Tests;

internal static class Platform
{
    public static async Task<int> RunTestAsync(Action<IServiceCollection> configure)
    {
        var builder = await TestApplication.CreateBuilderAsync(["--results-directory ./TestResults"]);
        configure(builder.AddBddDotNet());
        using var testApp = await builder.BuildAsync();
        return await testApp.RunAsync();
    }
}
