namespace DeepSpaceSaga.UI.Controller;

[ExcludeFromCodeCoverage]
public static class UiControllerStartupExtensions
{
    public static void AddUiControllerServices(this IServiceCollection services)
    {
        services.AddSingleton<IOuterSpaceService, OuterSpaceService>();
    }
}
