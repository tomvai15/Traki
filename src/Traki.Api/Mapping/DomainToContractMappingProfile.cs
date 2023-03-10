using AutoMapper;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Project;
using Traki.Api.Contracts.Question;
using Traki.Api.Contracts.Template;
using Traki.Api.Models;

namespace Traki.Api.Mapping
{
    public class DomainToContractMappingProfile: Profile
    {
        public DomainToContractMappingProfile()
        {
            AddTemplateMappings();
            AddQuestionMappings();
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

        public void AddQuestionMappings()
        {
            CreateMap<Question, GetQuestionResponse>()
                .ForMember(x => x.Question, opt => opt.MapFrom(x => x));

            CreateMap<IEnumerable<Question>, GetQuestionsResponse>()
                .ForMember(x => x.Questions, memberOptions: opt => opt.MapFrom(x => x));

            CreateMap<Question, QuestionDto>().ReverseMap();

            CreateMap<CreateQuestionRequest, Question>()
                .IncludeMembers(x => x.Question);

            CreateMap<UpdateQuestionRequest, Question>();
        }
    }
}
