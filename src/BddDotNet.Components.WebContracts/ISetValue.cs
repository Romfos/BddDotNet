namespace BddDotNet.Components.WebContracts;

public interface ISetValue<T>
{
    Task SetValueAsync(T value);
}
