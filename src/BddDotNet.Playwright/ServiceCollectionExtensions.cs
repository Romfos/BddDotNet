using BddDotNet.Playwright.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Playwright;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPlaywright(this IServiceCollection services)
    {
        services.TryAddScoped<IRouteService, RouteService>();
        services.SourceGeneratedGherkinSteps();

        return services;
    }

    public static IServiceCollection Component<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TComponent>(
        this IServiceCollection services, string path) where TComponent : class, IComponent
    {
        services.AddKeyedTransient<IComponent, TComponent>(path.Trim());
        return services;
    }
}
