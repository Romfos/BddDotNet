namespace BddDotNet.Playwright.Contracts.Contracts;

public interface IEnabled
{
    Task<bool> IsEnabledAsync();
}
