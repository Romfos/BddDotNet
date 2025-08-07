using BddDotNet.Components.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Components.Routing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ComponentRoutingSystem(this IServiceCollection services)
    {
        services.TryAddScoped<IRoutingService, RoutingService>();
        services.TryAddScoped<IOptionsService, OptionsService>();
        return services;
    }

    public static ComponentBuilder Component<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TComponent>(
        this IServiceCollection services, string path) where TComponent : class, IComponent
    {
        path = path.Trim();
        services.AddKeyedTransient<IComponent, TComponent>(path);
        return new(services, path);
    }

    public static IServiceCollection ComponentOptions<T>(
        this IServiceCollection services, string path, T value) where T : notnull
    {
        services.AddKeyedSingleton(path.Trim(), (_, path) => new ComponentOptions(value));
        return services;
    }
}
