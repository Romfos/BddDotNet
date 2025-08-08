namespace BddDotNet.Components.Web;

public interface IGetValue<T>
{
    Task<T> GetValueAsync();
}
