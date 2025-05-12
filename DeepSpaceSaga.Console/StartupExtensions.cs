using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace DeepSpaceSaga.Console
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        public static IServiceCollection AddClientControls(this IServiceCollection services)
        {
            
            return services;
        }
    }
}
