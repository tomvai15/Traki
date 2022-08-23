using Inventory.Infrastructure.Parts;

namespace Inventory.Api.Bootstratp
{
    public static class RepositoryBootstrap
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPartsRepository, PartsRepository>();
            return services;
        }
    }
}
