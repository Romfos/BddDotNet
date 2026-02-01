using BddDotNet.Extensibility;
using BddDotNet.Internal.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Builder;
using Microsoft.Testing.Platform.Capabilities.TestFramework;

namespace BddDotNet;

public static class TestApplicationBuilderExtensions
{
    public static IServiceCollection AddBddDotNet(this ITestApplicationBuilder builder)
    {
        var services = new ServiceCollection();

        services.AddSingleton<BddDotNetTestFramework>();

        services.AddScoped<TestContext>();
        services.AddScoped<ITestContext>(services => services.GetRequiredService<TestContext>());
        services.AddScoped<IScenarioContext, ScenarioContext>();

        services.AddScoped<ScenarioExecutionService>();
        services.AddScoped<StepExecutionService>();

        builder.RegisterTestFramework(
            _ => new TestFrameworkCapabilities(),
            (_, _) => new BddDotNetTestFramework(services));

        return services;
    }
}
