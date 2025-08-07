using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Routing;

public sealed class ComponentBuilder
{
    private readonly IServiceCollection services;
    private readonly string path;

    internal ComponentBuilder(IServiceCollection services, string path)
    {
        this.services = services;
        this.path = path;
    }

    public void Options<T>(T value) where T : notnull
    {
        services.ComponentOptions(path, value);
    }
}
