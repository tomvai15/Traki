using Traki.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Traki.Api.Bootstrapping
{
    public static class DatabaseBootstrap
    {
        public static IServiceCollection AddTrakiDbContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionStringSectionName = "TrakiDbContext";
            services.AddDbContext<TrakiDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString(connectionStringSectionName)));

            return services;
        }
    }
}
