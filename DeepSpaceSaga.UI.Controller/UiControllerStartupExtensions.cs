namespace DeepSpaceSaga.UI.Controller;

[ExcludeFromCodeCoverage]
public static class UiControllerStartupExtensions
{
    public static void AddUiControllerServices(this IServiceCollection services)
    {
        services.AddSingleton<IOuterSpaceService, OuterSpaceService>();
        services.AddSingleton<IGameManager, GameManager>();
        
        // Register MVP components
        services.AddMvpComponents();
    }

    private static void AddMvpComponents(this IServiceCollection services)
    {
        // Register Controllers (business logic layer)
        services.AddScoped<IMainMenuController, MainMenuController>();
        services.AddScoped<IGameMenuController, GameMenuController>();
            
        // Register Presenters (UI coordination layer)
        services.AddScoped<IGameMenuPresenter, GameMenuPresenter>();
    }
}
