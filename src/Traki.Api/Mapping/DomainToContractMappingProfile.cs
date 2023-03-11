﻿using AutoMapper;
using Traki.Api.Contracts.Checklist;
using Traki.Api.Contracts.ChecklistQuestion;
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
            AddChecklistMappings();
            AddChecklistQuestionMappings();
            AddChecklistQuestionMappings();
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

        public void AddChecklistMappings()
        {
            CreateMap<CheckList, GetChecklistResponse>()
                .ForMember(x => x.Checklist, opt => opt.MapFrom(x => x));

            CreateMap<IEnumerable<CheckList>, GetChecklistsResponse>()
                .ForMember(x => x.Checklists, opt => opt.MapFrom(x => x));

            CreateMap<CheckList, ChecklistDto>().ReverseMap();
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
