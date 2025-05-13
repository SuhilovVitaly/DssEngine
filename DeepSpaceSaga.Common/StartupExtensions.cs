using DeepSpaceSaga.Common.Abstractions.Session.Entities;
using DeepSpaceSaga.Common.Implementation.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DeepSpaceSaga.Controller;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        // Services 
        services.AddSingleton<ISessionInfo, SessionInfo>();            

        return services;
    }
}
