using BddDotNet.Arguments;
using BddDotNet.Gherkin.Models.Internal;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace BddDotNet.Gherkin.Models;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection ModelTransformation<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TModel>()
            where TModel : class
        {
            services.ArgumentTransformation<ModelArgumentTransformation<TModel>>();

            return services;
        }
    }
}
