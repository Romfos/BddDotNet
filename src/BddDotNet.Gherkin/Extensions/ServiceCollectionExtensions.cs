using BddDotNet.Extensions;
using BddDotNet.Gherkin.Models;
using BddDotNet.Gherkin.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BddDotNet.Gherkin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Given(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Task> method)
    {
        services.AddScoped(_ => new GherkinStep(StepType.Given, pattern, method));

        return services;
    }

    public static IServiceCollection When(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Task> method)
    {
        services.AddScoped(_ => new GherkinStep(StepType.When, pattern, method));

        return services;
    }

    public static IServiceCollection Then(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Task> method)
    {
        services.AddScoped(_ => new GherkinStep(StepType.Then, pattern, method));

        return services;
    }

    public static IServiceCollection Scenario<TFeatureType>(
        this IServiceCollection services,
        string feature,
        string scenario,
        Func<IScenarioContext, Task> method,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null)
    {
        if (filePath == null)
        {
            throw new ArgumentNullException(nameof(filePath), "File path cannot be null.");
        }
        if (lineNumber == null)
        {
            throw new ArgumentNullException(nameof(lineNumber), "Line number cannot be null.");
        }

        var type = typeof(TFeatureType);
        var assemblyName = type.Assembly.GetName().Name!;

        UseGherkin(services);

        services.TestCase(
            assemblyName,
            type.Namespace ?? assemblyName,
            feature,
            scenario,
            services => method(services.GetRequiredService<IScenarioContext>()),
            filePath,
            lineNumber.Value);

        return services;
    }

    private static void UseGherkin(this IServiceCollection services)
    {
        services.TryAddScoped<IScenarioContext, ScenarioContext>();
        services.TryAddScoped<StepExecutionService>();
    }
}
