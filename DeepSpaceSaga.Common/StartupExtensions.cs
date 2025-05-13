using DeepSpaceSaga.Common.Abstractions.Services;
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
        //  
        services.AddSingleton<ISessionInfo, SessionInfo>();
        services.AddSingleton<IGameFlowService, GameFlowService>();

        return services;
    }
}
