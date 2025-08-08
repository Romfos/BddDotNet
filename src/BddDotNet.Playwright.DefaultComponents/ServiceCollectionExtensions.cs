using BddDotNet.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Playwright;

namespace BddDotNet.Playwright.DefaultComponents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection SinglePageChromiumPlaywright(this IServiceCollection services, BrowserTypeLaunchOptions browserTypeLaunchOptions)
    {
        services.WebContracts();

        services.TryAddSingleton(_ =>
        {
            Program.Main(["install", "chromium"]);
            return Microsoft.Playwright.Playwright.CreateAsync().Result;
        });

        services.TryAddSingleton(browserTypeLaunchOptions);

        services.TryAddSingleton(services =>
        {
            var playwright = services.GetRequiredService<IPlaywright>();
            var browserTypeLaunchOptions = services.GetRequiredService<BrowserTypeLaunchOptions>();
            return playwright.Chromium.LaunchAsync(browserTypeLaunchOptions).Result;
        });

        services.TryAddSingleton(services =>
        {
            var browser = services.GetRequiredService<IBrowser>();
            return browser.NewPageAsync().Result;
        });

        services.SourceGeneratedGherkinSteps();

        return services;
    }
}
