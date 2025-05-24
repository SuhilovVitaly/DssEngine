using DeepSpaceSaga.Common.Tools;

namespace DeepSpaceSaga.Common;

[ExcludeFromCodeCoverage]
public static class StartupExtensions
{
    public static IServiceCollection AddCommonServices(this IServiceCollection services)
    {
        //          
        services.AddSingleton<IGenerationTool, GenerationTool>();

        return services;
    }
}
