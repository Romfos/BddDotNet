using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Configuration;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection Configuration(BddDotNetConfiguration configuration)
        {
            serviceCollection.AddSingleton(configuration);
            return serviceCollection;
        }
    }
}
