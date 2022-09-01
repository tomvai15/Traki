using Inventory.Api.Bootstratp;
using Inventory.Infrastructure.Bootstrapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api
{ 
    public static class Startup 
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHandlers(configuration)
                .AddRepositories(configuration)
                .AddInventoryDbContext(configuration, "InventoryDbContext");
            services.AddServiceAuth(configuration);

            return services;
        }
    }
}
