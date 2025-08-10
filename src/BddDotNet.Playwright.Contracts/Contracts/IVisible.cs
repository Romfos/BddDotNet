namespace BddDotNet.Playwright.Contracts.Contracts;

public interface IVisible
{
    Task<bool> IsVisibleAsync();
}
