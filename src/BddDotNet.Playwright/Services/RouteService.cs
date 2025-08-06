using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Playwright.Services;

internal sealed class RouteService(IServiceProvider serviceProvider) : IRouteService
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
            throw new Exception($"Component at path '{path}' is not implementing contract '{typeof(T).Name}'");
        }

        return contract;
    }
}
