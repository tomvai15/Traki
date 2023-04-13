using AutoMapper;
using Traki.Api.Contracts.Auth;
using Traki.Api.Contracts.Checklist;
using Traki.Api.Contracts.ChecklistQuestion;
using Traki.Api.Contracts.Company;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Project;
using Traki.Api.Contracts.Protocol;
using Traki.Api.Contracts.Recommendation;
using Traki.Api.Contracts.Section;
using Traki.Api.Contracts.Section.Items;
using Traki.Api.Contracts.Template;
using Traki.Api.Contracts.User;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Models.Section;
using Traki.Domain.Models.Section.Items;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Entities.Section;
using Question = Traki.Domain.Models.Section.Items.Question;

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
            AddSectionMapping();
           // AddQuestionMappings();
            AddChecklistMappings();
            AddChecklistQuestionMappings();
            AddChecklistQuestionMappings();
        }

        public void AddRecommendationMappings()
        {
            CreateMap<Recommendation, RecommendationDto>().ReverseMap();
            CreateMap<DefectRecomendation, DefectRecomendationDto>().ReverseMap();
        }

        public void AddDrawingMappings()
        {
            CreateMap<Drawing, DrawingDto>().ReverseMap();
            CreateMap<Defect, DefectDto>().ReverseMap();
            CreateMap<DefectComment, DefectCommentDto>().ReverseMap();
            CreateMap<StatusChange, StatusChangeDto>().ReverseMap();
            CreateMap<User, AuthorDto>().ReverseMap();
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

        public void AddSectionMapping()
        {
            CreateMap<Protocol, ProtocolDto>().ReverseMap();

            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<MultipleChoice, MultipleChoiceDto>().ReverseMap();
            CreateMap<TextInput, TextInputDto>().ReverseMap();
            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();

            CreateMap<Section, SectionDto>().ReverseMap();
            CreateMap<Checklist, ChecklistDto>().ReverseMap();

            CreateMap<Table, TableDto>().ReverseMap();
            CreateMap<TableRow, TableRowDto>().ReverseMap();
            CreateMap<RowColumn, RowColumnDto>().ReverseMap();
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

        /*
        public void AddQuestionMappings()
        {
            CreateMap<Question, GetQuestionResponse>()
                .ForMember(x => x.Question, opt => opt.MapFrom(x => x));

            CreateMap<IEnumerable<Question>, GetQuestionsResponse>()
                .ForMember(x => x.Questions, memberOptions: opt => opt.MapFrom(x => x));

            CreateMap<Question, OldQuestionDto>().ReverseMap();

            CreateMap<CreateQuestionRequest, Question>()
                .IncludeMembers(x => x.Question);

            CreateMap<UpdateQuestionRequest, Question>();
        }*/

        public void AddChecklistMappings()
        {
            CreateMap<CheckList, GetChecklistResponse>()
                .ForMember(x => x.Checklist, opt => opt.MapFrom(x => x));

            CreateMap<IEnumerable<CheckList>, GetChecklistsResponse>()
                .ForMember(x => x.Checklists, opt => opt.MapFrom(x => x));

            CreateMap<CheckList, OldChecklistDto>().ReverseMap();
        }

        public void AddChecklistQuestionMappings()
        {
            CreateMap<ChecklistQuestion, GetChecklistQuestionResponse>()
                .ForMember(x => x.ChecklistQuestion, opt => opt.MapFrom(x => x));

            CreateMap<IEnumerable<ChecklistQuestion>, GetChecklistQuestionsResponse>()
                .ForMember(x => x.ChecklistQuestions, opt => opt.MapFrom(x => x));

            CreateMap<ChecklistQuestion, ChecklistQuestionDto>().ReverseMap();
        }
    }
}
