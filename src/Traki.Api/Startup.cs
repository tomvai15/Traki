using Traki.Api.Bootstrapping;

namespace Traki.Api
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .AddMappingProfiles()
                .AddTrakiDbContext(configuration)
                .AddAuthServices(configuration)
                .AddHandlers(configuration)
                .AddRepositories(configuration)
                .AddHttpResponseMappings(configuration)
                .AddDocusignServices(configuration)
                .AddBlobStorageServices(configuration)
                .AddEmailServices(configuration)
                .AddCryptographyServices()
                .AddCorsPolicy();

            return services;
        }
    }
}
