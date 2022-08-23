using Inventory.Domain.Parts;

namespace Inventory.Api.Bootstratp
{
    public static class HandlerBootsrap
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IPartsHandler, PartsHandler>();
            return services;
        }
    }
}
