namespace BddDotNet.Components.Web.Contracts;

public interface ISetValue<T>
{
    Task SetValueAsync(T value);
}
