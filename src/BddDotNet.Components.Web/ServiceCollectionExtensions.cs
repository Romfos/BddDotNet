using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Web;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection WebContracts(this IServiceCollection services)
    {
        services.ComponentsFramework();

        services.SourceGeneratedGherkinSteps();

        return services;
    }
}
