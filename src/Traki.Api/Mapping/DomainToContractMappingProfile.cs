using AutoMapper;
using Traki.Api.Contracts.Company;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Project;
using Traki.Api.Contracts.Recommendation;
using Traki.Api.Contracts.Template;
using Traki.Api.Contracts.User;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;

namespace Traki.Api.Mapping
{
    public class DomainToContractMappingProfile: Profile
    {
        public DomainToContractMappingProfile()
        {
            AddRecommendationMappings();
            AddDrawingMappings();
            AddCompanyMappings();
            AddProductMappings();
            AddProjectMappings();
            AddUserMappings();
            AddTemplateMappings();
        }

        public void AddRecommendationMappings()
        {
            CreateMap<Recommendation, RecommendationDto>().ReverseMap();
            CreateMap<DefectRecomendation, DefectRecomendationDto>().ReverseMap();
            CreateMap<ProductRecomendation, ProductRecomendationDto>().ReverseMap();
        }

        public void AddDrawingMappings()
        {
            CreateMap<Drawing, DrawingDto>().ReverseMap();
            CreateMap<Defect, DefectDto>().ReverseMap();
            CreateMap<DefectComment, DefectCommentDto>().ReverseMap();
            CreateMap<StatusChange, StatusChangeDto>().ReverseMap();
            CreateMap<User, AuthorDto>().ReverseMap();
            CreateMap<DefectNotification, DefectNotificationDto>().ReverseMap();
        }

        public void AddCompanyMappings()
        {
            CreateMap<Company, GetCompanyResponse>()
                .ForMember(p => p.Company, opt => opt.MapFrom(p => p));

            CreateMap<Company, CompanyDto>().ReverseMap();
        }

        public void AddProductMappings()
        {
            CreateMap<Product, GetProductResponse>()
             .ForMember(p => p.Product, opt => opt.MapFrom(p => p));

            CreateMap<IEnumerable<Product>, GetProductsResponse>()
                .ForMember(p => p.Products, opt => opt.MapFrom(p => p));

            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<CreateProductRequest, Product>()
                .IncludeMembers(p => p.Product);
        }

        public void AddProjectMappings()
        {
            CreateMap<Project, GetProjectResponse>()
                .ForMember(p => p.Project, opt => opt.MapFrom(p => p));

            CreateMap<IEnumerable<Project>, GetProjectsResponse>()
                .ForMember(p => p.Projects, opt => opt.MapFrom(p => p));

            CreateMap<Project, ProjectDto>().ReverseMap();

            CreateMap<CreateProjectRequest, Project>()
                .IncludeMembers(p => p.Project);
        }

        public void AddUserMappings()
        {
            CreateMap<CreateUserRequest, User>();
            CreateMap<UserDto, User>().ReverseMap();
        }

        public void AddTemplateMappings()
        {
            CreateMap<Template, GetTemplateResponse>()
                .ForMember(x => x.Template, opt => opt.MapFrom(x => x));

            CreateMap<IEnumerable<Template>, GetTemplatesResponse>()
                .ForMember(x => x.Templates, memberOptions: opt => opt.MapFrom(x => x));

            CreateMap<Template, TemplateDto>().ReverseMap();

            CreateMap<CreateTemplateRequest, Template>()
                .IncludeMembers(x => x.Template);
        }
    }
}
