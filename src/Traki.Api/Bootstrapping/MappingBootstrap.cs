using AutoMapper;
using Traki.Api.Mapping;

namespace Traki.Api.Bootstrapping
{
    public static class MappingBootstrap
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new UserModelsMappingProfile());
            });

            services.AddSingleton(config.CreateMapper());
            return services;
        }
    }
}
