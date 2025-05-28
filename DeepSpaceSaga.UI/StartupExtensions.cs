using DeepSpaceSaga.Common.Abstractions.UI.Screens;
using DeepSpaceSaga.UI.Controller.Screens;
using DeepSpaceSaga.UI.Screens.MainMenu;

namespace DeepSpaceSaga.UI
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static void AddClientServices(this IServiceCollection services)
        {
            // Register core services first
            services.AddSingleton<IScreenResolution, ScreenResolution>();
            services.AddSingleton<IScreensService, ScreensService>();

            // Register screens after services
            services.AddClientScreens();
        }

        

        public static void AddClientScreens(this IServiceCollection services)
        {
            // Register ScreenTacticalMap as transient to create new instance each time
            services.AddTransient<ScreenTacticalMap>();
            
            // Register other screens as scoped to maintain state within a scope 
            services.AddScoped<ScreenMainMenu>();
            services.AddScoped<ScreenTacticGame>();
            services.AddScoped<ScreenGameMenu>();

            // Register main background screen last to avoid circular dependency
            services.AddScoped<ScreenBackground>();
        }
    }
}
