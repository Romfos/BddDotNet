using BddDotNet.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet;

public static partial class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection BeforeScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IBeforeScenario
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IBeforeScenario>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }

        public IServiceCollection AfterScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IAfterScenario
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IAfterScenario>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }

        public IServiceCollection ArgumentTransformation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IArgumentTransformation
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IArgumentTransformation>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }
    }
}
