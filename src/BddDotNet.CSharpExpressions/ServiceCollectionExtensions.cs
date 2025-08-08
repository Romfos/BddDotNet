using BddDotNet.CSharpExpressions.Internal;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BddDotNet.CSharpExpressions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection EnableCSharpExpressions<TGlobals>(this IServiceCollection services) where TGlobals : class
    {
        services.TryAddScoped<TGlobals>();
        services.TryAddSingleton(_ => ScriptOptions.Default);
        services.ArgumentTransformation<CSharpExpressionTransformation<TGlobals>>();

        return services;
    }
}
