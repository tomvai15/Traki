using AutoMapper;
using Traki.Domain.Models;
using Traki.Infrastructure.Entities;

namespace Traki.Api.Mapping
{
    public class EntityToDomainModelMappingProfile : Profile
    {
        public EntityToDomainModelMappingProfile()
        {
            CreateMap<Company, CompanyEntity>().ReverseMap();
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<Project, ProjectEntity>().ReverseMap();
            CreateMap<Product, ProductEntity>().ReverseMap();
            CreateMap<Template, TemplateEntity>().ReverseMap();
            CreateMap<Question, QuestionEntity>().ReverseMap();
            CreateMap<CheckList, ChecklistEntity>().ReverseMap();
            CreateMap<ChecklistQuestion, ChecklistQuestionEntity>().ReverseMap();
        }
    }
}
