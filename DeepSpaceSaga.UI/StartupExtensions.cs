using DeepSpaceSaga.UI.Screens.TacticalMap;

namespace DeepSpaceSaga.UI
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            // 
            services.AddSingleton<IGameContextService, GameContextService>();
            services.AddSingleton<IScreensService, ScreensService>();
            services.AddSingleton<GameManager>();
            return services;
        }

        public static IServiceCollection AddClientScreens(this IServiceCollection services)
        {
            services.AddScoped<ScreenTacticalMap>();
            services.AddScoped<ScreenBackground>();
            services.AddScoped<ScreenGameMenu>();
            services.AddScoped<ScreenTacticGame>();
            return services;
        }
    }
}
