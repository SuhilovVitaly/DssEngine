using DeepSpaceSaga.UI.Screens.MainMenu;
using DeepSpaceSaga.UI.Screens.TacticalMap;
using DeepSpaceSaga.UI.Screens.TacticalMap.ScreenControls;
using DeepSpaceSaga.UI.Controller;
using DeepSpaceSaga.UI.Controller.Abstractions;
using DeepSpaceSaga.UI.Presenters;

namespace DeepSpaceSaga.UI
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            // Register core services first
            services.AddSingleton<IScreensService, ScreensService>();
            services.AddSingleton<GameManager>();
            services.AddSingleton<IGameManager>(provider => provider.GetRequiredService<GameManager>());

            // Register MVP components
            services.AddMvpComponents();

            // Register screens after services
            services.AddClientScreens();

            return services;
        }

        public static IServiceCollection AddMvpComponents(this IServiceCollection services)
        {
            // Register Controllers (business logic layer)
            services.AddScoped<IMainMenuController, MainMenuController>();
            services.AddScoped<IGameMenuController, GameMenuController>();
            
            // Register Presenters (UI coordination layer)
            services.AddScoped<IMainMenuPresenter, MainMenuPresenter>();
            services.AddScoped<IGameMenuPresenter, GameMenuPresenter>();
            
            // TODO: Add other Controllers and Presenters here as they are created
            // services.AddScoped<ITacticalMapController, TacticalMapController>();
            // services.AddScoped<ITacticalMapPresenter, TacticalMapPresenter>();

            return services;
        }

        public static IServiceCollection AddClientScreens(this IServiceCollection services)
        {
            // Register ScreenTacticalMap as transient to create new instance each time
            services.AddTransient<ScreenTacticalMap>();
            
            // Register other screens as scoped to maintain state within a scope 
            services.AddScoped<ScreenMainMenu>();
            services.AddScoped<ScreenTacticGame>();
            services.AddScoped<ScreenGameMenu>();

            // Register main background screen last to avoid circular dependency
            services.AddScoped<ScreenBackground>();
            
            return services;
        }
    }
}
