using AutoMapper;
using Traki.Api.Mapping;

namespace Traki.UnitTests.Infrastructure.Common
{
    public static class DefaultMapper
    {
        public static IMapper CreateMapper()
        {
            IConfigurationProvider configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EntityToDomainModelMappingProfile());
                cfg.AddProfile(new DomainToContractMappingProfile());
            });

            return new Mapper(configuration);
        }
    }
}
