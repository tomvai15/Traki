using Traki.Api.Bootstrapping;

namespace Traki.Api
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMappingProfiles()
                .AddTrakiDbContext(configuration)
                .AddAuthorisationServices(configuration)
                .AddHandlers(configuration)
                .AddCryptographyServices()
                .AddCorsPolicy();

            return services;
        }
    }
}
