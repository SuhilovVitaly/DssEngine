namespace DeepSpaceSaga.Console;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        return services;
    }
}
