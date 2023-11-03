using Microsoft.Extensions.DependencyInjection;
using Petrucio.Domain.Services;

namespace Petrucio.Domain.Extensions
{
    public static class DependecyInjectionExtension
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<EntrypointService>();

            return services;
        }
    }
}
