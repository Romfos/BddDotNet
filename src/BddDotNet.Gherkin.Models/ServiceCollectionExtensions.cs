using BddDotNet.Gherkin.Models.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Gherkin.Models;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ModelTransformation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TModel>(
        this IServiceCollection services) where TModel : class
    {
        services.ArgumentTransformation<ModelArgumentTransformation<TModel>>();

        return services;
    }
}
