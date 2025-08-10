namespace BddDotNet.Playwright.Contracts.Contracts;

public interface IGetValue<T>
{
    Task<T> GetValueAsync();
}
