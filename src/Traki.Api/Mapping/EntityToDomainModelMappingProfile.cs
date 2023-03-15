﻿using AutoMapper;
using Traki.Api.Data.Entities;
using Traki.Api.Models;

namespace Traki.Api.Mapping
{
    public class EntityToDomainModelMappingProfile : Profile
    {
        public EntityToDomainModelMappingProfile()
        {
            CreateMap<Project, ProjectEntity>().ReverseMap();
            CreateMap<Product, ProductEntity>().ReverseMap();
            CreateMap<Template, TemplateEntity>().ReverseMap();
            CreateMap<Question, QuestionEntity>().ReverseMap();
            CreateMap<CheckList, ChecklistEntity>().ReverseMap();
            CreateMap<ChecklistQuestion, ChecklistQuestionEntity>().ReverseMap();
        }
    }
}
