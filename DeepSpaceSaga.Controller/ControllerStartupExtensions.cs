namespace DeepSpaceSaga.Controller;

[ExcludeFromCodeCoverage]
public static class ControllerStartupExtensions
{
    public static IServiceCollection AddControllerServices(this IServiceCollection services)
    {
        // Services 
        services.AddTransient<IExecutor, Executor>();
        services.AddScoped<IWorkerService, WorkerService>();            

        return services;
    }
}
