namespace BddDotNet.Playwright;

public interface IRouteService
{
    T GetComponent<T>(string path) where T : class;
}
