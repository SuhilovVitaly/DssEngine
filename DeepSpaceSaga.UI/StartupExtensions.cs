using DeepSpaceSaga.UI.Screens.GameMenu;

namespace DeepSpaceSaga.UI
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IScreensService, ScreensService>();
            services.AddSingleton<GameManager>();
            return services;
        }

        public static IServiceCollection AddClientScreens(this IServiceCollection services)
        {
            services.AddScoped<ScreenBackground>();
            services.AddScoped<ScreenGameMenu>();
            return services;
        }
    }
}
