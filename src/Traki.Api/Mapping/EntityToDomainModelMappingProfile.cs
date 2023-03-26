using AutoMapper;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Models.Section.Items;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities.Section.Items;
using Question = Traki.Domain.Models.Section.Items.Question;

namespace Traki.Api.Mapping
{
    public class EntityToDomainModelMappingProfile : Profile
    {
        public EntityToDomainModelMappingProfile()
        {
            CreateMap<Protocol, ProtocolEntity>().ReverseMap();

            CreateMap<Company, CompanyEntity>().ReverseMap();
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<Project, ProjectEntity>().ReverseMap();
            CreateMap<Product, ProductEntity>().ReverseMap();
            CreateMap<Template, TemplateEntity>().ReverseMap();
            //CreateMap<Question, OldQuestionEntity>().ReverseMap();
            CreateMap<CheckList, OldChecklistEntity>().ReverseMap();
            CreateMap<ChecklistQuestion, ChecklistQuestionEntity>().ReverseMap();

            CreateMap<ChecklistEntity, Checklist>().ReverseMap();
            CreateMap<SectionEntity, Section>().ReverseMap();
            CreateMap<TableEntity, Table>().ReverseMap();

            CreateMap<ItemEntity, Item>().ReverseMap();
            CreateMap<MultipleChoiceEntity, MultipleChoice>().ReverseMap();
            CreateMap<OptionEntity, Option>().ReverseMap();
            CreateMap<QuestionEntity, Question>().ReverseMap();
            CreateMap<TextInputEntity, TextInput>().ReverseMap();
        }
    }
}
