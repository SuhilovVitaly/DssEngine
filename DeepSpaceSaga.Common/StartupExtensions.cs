namespace DeepSpaceSaga.Controller;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        //  
        services.AddSingleton<IMetricsService, MetricsService>();
        
        
        services.AddSingleton<ISaveLoadService, SaveLoadService>();
        //

        return services;
    }
}
