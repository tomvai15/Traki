using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Infrastructure.Bootstrapping
{
    public static class InventoryDbContextBootsratp
    {
        public static IServiceCollection AddInventoryDbContext(this IServiceCollection services, IConfiguration configuration, string connectionStringName)
        {
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(connectionStringName)));

            return services;
        }
    }
}
