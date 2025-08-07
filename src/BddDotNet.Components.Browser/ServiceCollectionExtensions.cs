using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Browser;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnableBrowserContracts(this IServiceCollection services)
    {
        services.SourceGeneratedGherkinSteps();

        return services;
    }
}
