using AutoMapper;
using Traki.Api.Contracts.Project;
using Traki.Api.Models.Project;

namespace Traki.Api.Mapping
{
    public class ProjectModelsMappingProfile : Profile
    {
        public ProjectModelsMappingProfile()
        {
            CreateMap<Project, GetProjectResponse>()
                .ForMember(p => p.Project, opt => opt.MapFrom(p => p));
            CreateMap<Project, ProjectDto>();
        }
    }
}
