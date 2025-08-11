using BddDotNet.Gherkin.CSharpExpressions.Internal;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BddDotNet.Gherkin.CSharpExpressions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection CSharpExpressions<TGlobals>(this IServiceCollection services, ScriptOptions? scriptOptions = null) where TGlobals : class
    {
        services.TryAddScoped<TGlobals>();
        services.TryAddSingleton(_ => scriptOptions ?? ScriptOptions.Default);
        services.ArgumentTransformation<CSharpExpressionTransformation<TGlobals>>();

        return services;
    }
}
