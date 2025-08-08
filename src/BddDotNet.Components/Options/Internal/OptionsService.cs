using BddDotNet.Components.Options;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Components.Options.Internal;

internal sealed class OptionsService(IServiceProvider serviceProvider) : IOptionsService
{
    public T GetOptions<T>(string path)
    {
        if (serviceProvider.GetKeyedService<ComponentOptions>(path.Trim())?.Value is not T value)
        {
            throw new Exception($"Invalid options for '{path}'");
        }

        return value;
    }
}
