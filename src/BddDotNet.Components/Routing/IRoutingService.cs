namespace BddDotNet.Components.Routing;

public interface IRoutingService
{
    T GetComponent<T>(string path) where T : class;
}
