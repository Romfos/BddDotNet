using BddDotNet.Internal.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace BddDotNet;

public static partial class ServiceCollectionExtensions
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
    }
}
