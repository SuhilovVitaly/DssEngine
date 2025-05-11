using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace DeepSpaceSaga.Server
{
    [ExcludeFromCodeCoverage]
    public static class ServerStartupExtensions
    {
        public static IServiceCollection AddServerServices(this IServiceCollection services)
        {
            // Services 
            services.AddScoped<IGameServer, LocalGameServer>();

            return services;
        }
    }
}
