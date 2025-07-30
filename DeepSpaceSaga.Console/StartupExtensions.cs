using DeepSpaceSaga.Common.Abstractions.UI;
using DeepSpaceSaga.Common.Abstractions.UI.Screens;
using DeepSpaceSaga.Console.Screens;
using DeepSpaceSaga.Console.Services.Screens;

namespace DeepSpaceSaga.Console;

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddConsoleServices(this IServiceCollection services)
    {
        services.AddSingleton<IScreenResolution, ConsoleScreenResolution>();
        services.AddTransient<IScreenTacticalMap, ScreenTacticalMap>();
        services.AddSingleton<IScreensService, ConsoleScreensService>();
        
        return services;
    }
}
