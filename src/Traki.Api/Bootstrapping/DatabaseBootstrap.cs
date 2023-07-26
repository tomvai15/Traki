using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Data;

namespace Traki.Api.Bootstrapping
{
    public static class DatabaseBootstrap
    {
        public static IServiceCollection AddTrakiDbContext(this IServiceCollection services, IConfiguration config)
        {
            const string connectionStringSectionName = "TrakiDbContextCloud";
            services.AddDbContext<TrakiDbContext>(options =>
                    {
                        options.UseSqlServer(config.GetConnectionString(connectionStringSectionName));
                        options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    }
                );

            return services;
        }
    }
}
