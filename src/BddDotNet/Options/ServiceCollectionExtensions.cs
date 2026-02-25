using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Options;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection Configure(BddDotNetOptions options)
        {
            serviceCollection.AddSingleton(options);
            return serviceCollection;
        }
    }
}
