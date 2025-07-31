using BddDotNet.Extensibility;
using BddDotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace BddDotNet;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Scenario<TFeature>(
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

        var type = typeof(TFeature);
        var assemblyName = type.Assembly.GetName().Name!;

        services.Scenario(assemblyName, type.Namespace ?? assemblyName, feature, scenario, filePath, lineNumber.Value, method);

        return services;
    }

    public static IServiceCollection Scenario(
        this IServiceCollection services,
        string assemblyName,
        string @namespace,
        string feature,
        string scenario,
        string filePath,
        int lineNumber,
        Func<IScenarioContext, Task> method)
    {
        services.AddSingleton(new Scenario(assemblyName, @namespace, feature, scenario, method, filePath, lineNumber));

        return services;
    }

    public static IServiceCollection Given(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Task> method)
    {
        services.AddScoped(_ => new Step(StepType.Given, pattern, method));

        return services;
    }

    public static IServiceCollection When(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Task> method)
    {
        services.AddScoped(_ => new Step(StepType.When, pattern, method));

        return services;
    }

    public static IServiceCollection Then(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Task> method)
    {
        services.AddScoped(_ => new Step(StepType.Then, pattern, method));

        return services;
    }

    public static IServiceCollection BeforeScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceCollection service) where T : class, IBeforeScenario
    {
        service.AddScoped<IBeforeScenario, T>();
        return service;
    }
}
