using BddDotNet.Components.Options;
using BddDotNet.Components.Options.Internal;
using BddDotNet.Components.Routing;
using BddDotNet.Components.Routing.Internal.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Components;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ComponentsFramework(this IServiceCollection services)
    {
        services.TryAddScoped<IOptionsService, OptionsService>();
        services.TryAddScoped<IRoutingService, RoutingService>();
        return services;
    }

    public static ComponentRoutingBuilder Component<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TComponent>(
        this IServiceCollection services, string path) where TComponent : class, IComponent
    {
        return Component(services, path, typeof(TComponent));
    }

    public static ComponentRoutingBuilder Component(this IServiceCollection services, string path,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type componentType)
    {
        path = path.SanitizePath();
        services.AddKeyedTransient(typeof(IComponent), path, componentType);
        return new(services, path);
    }

    public static IServiceCollection ComponentOptions(this IServiceCollection services, string path, object value)
    {
        path = path.SanitizePath();
        services.AddKeyedSingleton(path.Trim(), (_, _) => new ComponentOptions(value));
        return services;
    }
}
