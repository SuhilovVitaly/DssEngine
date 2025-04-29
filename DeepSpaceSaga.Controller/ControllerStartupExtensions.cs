using DeepSpaceSaga.Common.Abstractions.Services;
using DeepSpaceSaga.Controller.GameLoopTools;
using Microsoft.Extensions.DependencyInjection;

namespace DeepSpaceSaga.Controller
{
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
}
