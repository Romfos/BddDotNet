using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Routing;

public sealed class ComponentRoutingBuilder
{
    private readonly IServiceCollection services;
    private readonly string path;

    internal ComponentRoutingBuilder(IServiceCollection services, string path)
    {
        this.services = services;
        this.path = path;
    }

    public void Options(object value)
    {
        services.ComponentOptions(path, value);
    }
}
