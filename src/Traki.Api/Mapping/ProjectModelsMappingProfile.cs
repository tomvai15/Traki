using AutoMapper;
using Traki.Api.Contracts.Project;
using Traki.Api.Models;

namespace Traki.Api.Mapping
{
    public class ProjectModelsMappingProfile : Profile
    {
        public ProjectModelsMappingProfile()
        {
            CreateMap<Project, GetProjectResponse>()
                .ForMember(p => p.Project, opt => opt.MapFrom(p => p));

            CreateMap<IEnumerable<Project>, GetProjectsResponse>()
                .ForMember(p => p.Projects, opt => opt.MapFrom(p => p));

            CreateMap<Project, ProjectDto>().ReverseMap();

            CreateMap<CreateProjectRequest, Project>()
                .IncludeMembers(p => p.Project);
        }
    }
}
