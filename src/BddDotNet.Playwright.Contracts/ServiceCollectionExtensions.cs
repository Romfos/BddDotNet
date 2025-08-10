using BddDotNet.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BddDotNet.Playwright.Contracts;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection PlaywrightContracts(this IServiceCollection services)
    {
        services.ComponentsFramework();

        services.SourceGeneratedGherkinSteps();

        return services;
    }
}
