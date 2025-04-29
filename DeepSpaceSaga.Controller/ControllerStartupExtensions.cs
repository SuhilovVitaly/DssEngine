namespace DeepSpaceSaga.Controller;

public static class ControllerStartupExtensions
{
    public static IServiceCollection AddControllerServices(this IServiceCollection services)
    {
        // Services 
        services.AddTransient<Executor, Executor>();
        services.AddScoped<IWorkerService, WorkerService>();            

        return services;
    }
}
