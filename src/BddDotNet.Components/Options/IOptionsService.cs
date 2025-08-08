namespace BddDotNet.Components.Options;

public interface IOptionsService
{
    T GetOptions<T>(string path);
}
