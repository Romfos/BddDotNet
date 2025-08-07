namespace BddDotNet.Components.Routing;

public interface IOptionsService
{
    T GetOptions<T>(string path);
}
