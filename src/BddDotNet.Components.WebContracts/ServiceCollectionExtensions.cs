using BddDotNet.Components.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.WebContracts;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnableWebContracts(this IServiceCollection services)
    {
        services.EnableComponentRouting();

        services.SourceGeneratedGherkinSteps();

        return services;
    }
}
