using Microsoft.Extensions.DependencyInjection;

namespace Petrucio.Infra
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
