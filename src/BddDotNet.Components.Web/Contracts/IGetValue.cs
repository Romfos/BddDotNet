namespace BddDotNet.Components.Web.Contracts;

public interface IGetValue<T>
{
    Task<T> GetValueAsync();
}
