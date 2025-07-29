using BddDotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace BddDotNet.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection BeforeTestCase(this IServiceCollection service, Func<IServiceProvider, Task> method)
    {
        service.AddScoped(_ => new BeforeTestCaseHook(method));
        return service;
    }

    public static IServiceCollection TestCase<TTestCaseType>(
        this IServiceCollection services,
        string name,
        Func<IServiceProvider, Task> method,
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

        var type = typeof(TTestCaseType);
        var assemblyName = type.Assembly.GetName().Name!;
        TestCase(services, assemblyName, type.Namespace ?? assemblyName, type.FullName!, name, method, filePath, lineNumber.Value);

        return services;
    }

    public static IServiceCollection TestCase(
        this IServiceCollection services,
        string assemblyName,
        string @namespace,
        string testCaseTypeName,
        string testCaseName,
        Func<IServiceProvider, Task> method,
        string filePath,
        int lineNumber)
    {
        services.AddSingleton(new TestCase(assemblyName, @namespace, testCaseTypeName, testCaseName, method, filePath, lineNumber));
        return services;
    }
}
