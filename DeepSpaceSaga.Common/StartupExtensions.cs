namespace DeepSpaceSaga.Controller;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        //  
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<ISessionInfoService, SessionInfoService>();
        services.AddSingleton<IGameFlowService, GameFlowService>();

        return services;
    }
}
