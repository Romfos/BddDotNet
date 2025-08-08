using BddDotNet.Components.Options.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BddDotNet.Components.Options;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnableComponentOptions(this IServiceCollection services)
    {
        services.TryAddScoped<IOptionsService, OptionsService>();
        return services;
    }

    public static IServiceCollection ComponentOptions<T>(
        this IServiceCollection services, string path, T value) where T : notnull
    {
        services.AddKeyedSingleton(path.Trim(), (_, path) => new ComponentOptions(value));
        return services;
    }
}
