using BddDotNet.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BddDotNet.Components.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection CollectComponentsAndOptions<T>(this IServiceCollection services, string? prefix = null) where T : class
    {
        CollectComponentsAndOptions(services, prefix, typeof(T));

        return services;
    }

    private static void CollectComponentsAndOptions(IServiceCollection services, string? prefix, Type type)
    {
        foreach (var property in type.GetProperties())
        {
            if (property.GetCustomAttribute<RouteAttribute>() is RouteAttribute routeAttribute)
            {
                var path = prefix == null ? routeAttribute.Name : $"{prefix}>{routeAttribute.Name}";

                if (typeof(IComponent).IsAssignableFrom(property.PropertyType))
                {
                    services.Component(path, property.PropertyType);

                    if (property.GetCustomAttribute<OptionsAttribute>() is OptionsAttribute optionsAttribute)
                    {
                        services.ComponentOptions(path, optionsAttribute.Value);
                    }
                }
                else
                {
                    CollectComponentsAndOptions(services, path, property.PropertyType);
                }
            }
        }
    }
}
