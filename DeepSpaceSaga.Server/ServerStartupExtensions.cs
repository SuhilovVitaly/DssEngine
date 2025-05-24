namespace DeepSpaceSaga.Server;

[ExcludeFromCodeCoverage]
public static class ServerStartupExtensions
{
    public static IServiceCollection AddServerServices(this IServiceCollection services)
    {
        services.AddSingleton<IGameServer, LocalGameServer>();
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<ISchedulerService, SchedulerService>();
        services.AddSingleton<ISessionContextService, SessionContextService>();
        services.AddSingleton<ISessionInfoService, SessionInfoService>();

        return services;
    }
}
