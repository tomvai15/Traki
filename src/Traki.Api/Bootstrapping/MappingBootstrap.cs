using AutoMapper;
using Traki.Api.Mapping;
using Traki.Infrastructure.Mapping;

namespace Traki.Api.Bootstrapping
{
    public static class MappingBootstrap
    {
        public static IServiceCollection AddMappingProfiles(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainToInfrastructureMap());
                cfg.AddProfile(new DomainToContractMappingProfile());
                cfg.AddProfile(new ProtocolMappingProfile());
            });

            services.AddSingleton(config.CreateMapper());
            return services;
        }
    }
}
