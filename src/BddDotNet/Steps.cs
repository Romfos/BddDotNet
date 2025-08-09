using BddDotNet.Internal.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace BddDotNet;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection Given(this IServiceCollection services, Regex pattern, Delegate method)
    {
        services.AddScoped(_ => new Step(StepType.Given, pattern, _ => method));

        return services;
    }

    public static IServiceCollection Given(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Delegate> factory)
    {
        services.AddScoped(_ => new Step(StepType.Given, pattern, factory));

        return services;
    }

    public static IServiceCollection When(this IServiceCollection services, Regex pattern, Delegate method)
    {
        services.AddScoped(_ => new Step(StepType.When, pattern, _ => method));

        return services;
    }

    public static IServiceCollection When(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Delegate> factory)
    {
        services.AddScoped(_ => new Step(StepType.When, pattern, factory));

        return services;
    }

    public static IServiceCollection Then(this IServiceCollection services, Regex pattern, Delegate method)
    {
        services.AddScoped(_ => new Step(StepType.Then, pattern, _ => method));

        return services;
    }

    public static IServiceCollection Then(this IServiceCollection services, Regex pattern, Func<IServiceProvider, Delegate> factory)
    {
        services.AddScoped(_ => new Step(StepType.Then, pattern, factory));

        return services;
    }
}
