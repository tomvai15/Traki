using Inventory.Domain.Parts;
using Inventory.Domain.UserAuthentication;

namespace Inventory.Api.Bootstratp
{
    public static class HandlerBootsrap
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<IPartsHandler, PartsHandler>()
                .AddTransient<IUserAuthHandler, UserAuthHandler>();
            return services;
        }
    }
}
