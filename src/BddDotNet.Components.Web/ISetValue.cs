namespace BddDotNet.Components.Web;

public interface ISetValue<T>
{
    Task SetValueAsync(T value);
}
