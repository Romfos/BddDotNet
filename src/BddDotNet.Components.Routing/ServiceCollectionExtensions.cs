using BddDotNet.Components.Options;
using BddDotNet.Components.Routing.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Components.Routing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnableComponentRouting(this IServiceCollection services)
    {
        services.EnableComponentOptions();
        services.TryAddScoped<IRoutingService, RoutingService>();
        return services;
    }

    public static ComponentRoutingBuilder Component<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TComponent>(
        this IServiceCollection services, string path) where TComponent : class, IComponent
    {
        path = path.Trim();
        services.AddKeyedTransient<IComponent, TComponent>(path);
        return new(services, path);
    }
}
