using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Routing.Internal;

internal sealed class RoutingService(IServiceProvider serviceProvider) : IRoutingService
{
    public T GetComponent<T>(string path) where T : class
    {
        var component = serviceProvider.GetKeyedService<IComponent>(path.Trim());

        if (component == null)
        {
            throw new Exception($"Unable to locate component by path '{path}'");
        }

        if (component is not T contract)
        {
            throw new Exception($"Component at path '{path}' is not implementing '{typeof(T).Name}'");
        }

        return contract;
    }
}
