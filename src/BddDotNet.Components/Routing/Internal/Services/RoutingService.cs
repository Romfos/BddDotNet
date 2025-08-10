using BddDotNet.Components.Routing.Internal.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Routing.Internal.Services;

internal sealed class RoutingService(IServiceProvider serviceProvider) : IRoutingService
{
    public T GetComponent<T>(string path) where T : class
    {
        path = path.SanitizePath();
        var component = serviceProvider.GetKeyedService<IComponent>(path);

        if (component == null)
        {
            throw new UnableToLocateComponentException(path);
        }

        if (component is not T contract)
        {
            throw new UnsupportedContractException(path, typeof(T).Name);
        }

        return contract;
    }
}
