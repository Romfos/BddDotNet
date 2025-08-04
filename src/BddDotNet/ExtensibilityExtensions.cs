using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection BeforeScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceCollection service) where T : class, IBeforeScenario
    {
        service.TryAddScoped<T>();
        service.AddScoped<IBeforeScenario>(services => services.GetRequiredService<T>());
        return service;
    }

    public static IServiceCollection AfterScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceCollection service) where T : class, IAfterScenario
    {
        service.TryAddScoped<T>();
        service.AddScoped<IAfterScenario>(services => services.GetRequiredService<T>());
        return service;
    }

    public static IServiceCollection ArgumentTransformation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceCollection service) where T : class, IArgumentTransformation
    {
        service.TryAddScoped<T>();
        service.AddScoped<IArgumentTransformation>(services => services.GetRequiredService<T>());
        return service;
    }
}
