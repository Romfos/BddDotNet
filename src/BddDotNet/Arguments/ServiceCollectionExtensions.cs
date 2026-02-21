using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Arguments;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection ArgumentTransformation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IArgumentTransformation
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IArgumentTransformation>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }
    }
}
