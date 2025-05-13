namespace DeepSpaceSaga.UI
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddClientControls(this IServiceCollection services)
        {
            services.AddSingleton<GameManager>();
            return services;
        }
    }
}
