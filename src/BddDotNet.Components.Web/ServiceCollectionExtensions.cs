using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Web;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnableWebContracts(this IServiceCollection services)
    {
        services.EnableComponents();

        services.SourceGeneratedGherkinSteps();

        return services;
    }
}
