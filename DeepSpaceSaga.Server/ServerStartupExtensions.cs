namespace DeepSpaceSaga.Server
{
    [ExcludeFromCodeCoverage]
    public static class ServerStartupExtensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services)
        {
            // Services 
            services.AddScoped<ISessionContext, SessionContext>();
            services.AddSingleton<IGameServer, LocalGameServer>();
            services.AddSingleton<ISessionInfoService, SessionInfoService>();
            services.AddSingleton<ISchedulerService, SchedulerService>();

            return services;
        }
    }
}
