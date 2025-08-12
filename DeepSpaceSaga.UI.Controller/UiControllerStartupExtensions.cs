using DeepSpaceSaga.UI.Controller.Services.Localization;

namespace DeepSpaceSaga.UI.Controller;

[ExcludeFromCodeCoverage]
public static class UiControllerStartupExtensions
{
    public static void AddUiControllerServices(this IServiceCollection services)
    {
        services.AddSingleton<IOuterSpaceService, OuterSpaceService>();        
        services.AddSingleton<IGameManager, GameManager>();
        services.AddSingleton<IGameEventsService, GameEventsService>();
        services.AddSingleton<ISettings, Settings>();
        services.AddSingleton<ILocalizationService, LocalizationService>();

        // Register MVP components ILocalizationService
        services.AddMvpComponents();
    }

    private static void AddMvpComponents(this IServiceCollection services)
    {
        // Register Controllers (business logic layer)
        services.AddScoped<IMainMenuController, MainMenuController>();
        services.AddScoped<IGameMenuController, GameMenuController>();
        services.AddScoped<IScreenTacticalMapController, ScreenTacticalMapController>();

        // Register Presenters (UI coordination layer)
        services.AddScoped<IGameMenuPresenter, GameMenuPresenter>();
    }
}
