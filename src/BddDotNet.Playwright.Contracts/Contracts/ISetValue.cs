namespace BddDotNet.Playwright.Contracts.Contracts;

public interface ISetValue<T>
{
    Task SetValueAsync(T value);
}
