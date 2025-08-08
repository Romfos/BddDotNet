namespace BddDotNet.Components.WebContracts;

public interface IGetValue<T>
{
    Task<T> GetValueAsync();
}
