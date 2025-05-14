using DeepSpaceSaga.Server.Services;

namespace DeepSpaceSaga.Controller;

[ExcludeFromCodeCoverage]
public static class ControllerStartupExtensions
{
    public static IServiceCollection AddControllerServices(this IServiceCollection services)
    {
        // Services 
        services.AddTransient<ITurnSchedulerService, TurnSchedulerService>();
        

        return services;
    }
}
