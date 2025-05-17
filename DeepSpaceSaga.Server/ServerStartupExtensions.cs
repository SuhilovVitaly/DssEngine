using DeepSpaceSaga.Server.Services.Metrics;
using DeepSpaceSaga.Server.Services.Scheduler;
using DeepSpaceSaga.Server.Services.SessionContext;
using DeepSpaceSaga.Server.Services.SessionInfo;

namespace DeepSpaceSaga.Server
{
    [ExcludeFromCodeCoverage]
    public static class ServerStartupExtensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services)
        {
            // Services 
            services.AddSingleton<IMetricsService, MetricsService>();
            services.AddSingleton<ISessionInfoService, SessionInfoService>();
            services.AddScoped<ISessionContextService, SessionContextService>();
            services.AddSingleton<IGameServer, LocalGameServer>();
            services.AddSingleton<ISchedulerService, SchedulerService>();

            return services;
        }
    }
}
