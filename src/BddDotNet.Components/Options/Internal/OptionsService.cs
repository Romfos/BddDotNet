using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Options.Internal;

internal sealed class OptionsService(IServiceProvider serviceProvider) : IOptionsService
{
    public T GetOptions<T>(string path)
    {
        path = path.SanitizePath();
        if (serviceProvider.GetKeyedService<ComponentOptions>(path)?.Value is not T value)
        {
            throw new Exception($"Invalid options for '{path}'");
        }

        return value;
    }
}
