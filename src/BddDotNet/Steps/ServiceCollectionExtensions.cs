using BddDotNet.Internal.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace BddDotNet.Steps;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection Given(Regex pattern, Delegate method)
        {
            serviceCollection.AddScoped(_ => new Step(StepType.Given, pattern, _ => method));

            return serviceCollection;
        }

        public IServiceCollection Given(Regex pattern, Func<IServiceProvider, Delegate> factory)
        {
            serviceCollection.AddScoped(_ => new Step(StepType.Given, pattern, factory));

            return serviceCollection;
        }

        public IServiceCollection When(Regex pattern, Delegate method)
        {
            serviceCollection.AddScoped(_ => new Step(StepType.When, pattern, _ => method));

            return serviceCollection;
        }

        public IServiceCollection When(Regex pattern, Func<IServiceProvider, Delegate> factory)
        {
            serviceCollection.AddScoped(_ => new Step(StepType.When, pattern, factory));

            return serviceCollection;
        }

        public IServiceCollection Then(Regex pattern, Delegate method)
        {
            serviceCollection.AddScoped(_ => new Step(StepType.Then, pattern, _ => method));

            return serviceCollection;
        }

        public IServiceCollection Then(Regex pattern, Func<IServiceProvider, Delegate> factory)
        {
            serviceCollection.AddScoped(_ => new Step(StepType.Then, pattern, factory));

            return serviceCollection;
        }

        public IServiceCollection BeforeStep<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IBeforeStep
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IBeforeStep>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }

        public IServiceCollection AfterStep<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IAfterStep
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IAfterStep>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }

        public IServiceCollection Fallback<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IStepFallback
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IStepFallback>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }
    }
}
