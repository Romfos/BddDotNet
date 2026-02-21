using BddDotNet.Internal.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace BddDotNet.Scenarios;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection Scenario<TFeature>(
            string feature,
            string scenario,
            Func<IScenarioService, Task> method,
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

            serviceCollection.Scenario(assemblyName, type.Namespace ?? assemblyName, feature, scenario, filePath, lineNumber.Value, method);

            return serviceCollection;
        }

        public IServiceCollection Scenario(
            string assemblyName,
            string @namespace,
            string feature,
            string scenario,
            string filePath,
            int lineNumber,
            Func<IScenarioService, Task> method)
        {
            serviceCollection.AddSingleton(new Scenario(assemblyName, @namespace, feature, scenario, method, filePath, lineNumber));

            return serviceCollection;
        }

        public IServiceCollection BeforeScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IBeforeScenario
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IBeforeScenario>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }

        public IServiceCollection AfterScenario<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
            where T : class, IAfterScenario
        {
            serviceCollection.TryAddScoped<T>();
            serviceCollection.AddScoped<IAfterScenario>(services => services.GetRequiredService<T>());
            return serviceCollection;
        }
    }
}
