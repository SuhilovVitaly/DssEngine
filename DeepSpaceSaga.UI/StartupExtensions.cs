using DeepSpaceSaga.UI.Screens.TacticalMap;
using DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;

namespace DeepSpaceSaga.UI
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            // Register core services first
            services.AddSingleton<IGameContextService, GameContextService>();
            services.AddSingleton<IScreensService, ScreensService>();
            services.AddSingleton<GameManager>();

            // Register screens after services
            services.AddClientScreens();

            return services;
        }

        public static IServiceCollection AddClientScreens(this IServiceCollection services)
        {
            // Register screens as scoped to maintain state within a scope
            services.AddScoped<ScreenTacticalMap>();
            services.AddScoped<ScreenGameMenu>();
            services.AddScoped<ScreenTacticGame>();
            
            // Register main background screen last to avoid circular dependency
            services.AddScoped<ScreenBackground>();
            
            return services;
        }
    }
}
