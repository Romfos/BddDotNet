using BddDotNet.Internal.Services;
using BddDotNet.Options;
using BddDotNet.Scenarios;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Testing.Platform.Builder;
using Microsoft.Testing.Platform.Capabilities.TestFramework;

namespace BddDotNet;

public static class TestApplicationBuilderExtensions
{
    extension(ITestApplicationBuilder builder)
    {
        public IServiceCollection AddBddDotNet()
        {
            var services = new ServiceCollection();

            services.AddSingleton<BddDotNetTestFramework>();
            services.AddSingleton<BddDotNetOptions>();

            services.AddScoped<IScenarioService, ScenarioService>();
            services.AddScoped<ScenarioExecutionService>();
            services.AddScoped<StepExecutionService>();

            builder.RegisterTestFramework(
                _ => new TestFrameworkCapabilities(),
                (_, _) => new BddDotNetTestFramework(services));

            return services;
        }
    }
}
