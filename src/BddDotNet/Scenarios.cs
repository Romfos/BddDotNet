using BddDotNet.Extensibility;
using BddDotNet.Internal.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace BddDotNet;

public static partial class ServiceCollectionExtensions
{
    extension(IServiceCollection serviceCollection)
    {
        public IServiceCollection Scenario<TFeature>(
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
            Func<IScenarioContext, Task> method)
        {
            serviceCollection.AddSingleton(new Scenario(assemblyName, @namespace, feature, scenario, method, filePath, lineNumber));

            return serviceCollection;
        }
    }
}
