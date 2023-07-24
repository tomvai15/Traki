using AutoMapper;
using Traki.Api.Contracts.Protocol;
using Traki.Api.Contracts.Section.Items;
using Traki.Api.Contracts.Section;
using Traki.Domain.Models.Items;
using Traki.Domain.Models;

namespace Traki.Api.Mapping
{
    public class ProtocolMappingProfile : Profile
    {
        public ProtocolMappingProfile()
        {
            CreateMap<Protocol, ProtocolDto>()
                .ForMember(x => x.Tables, x => x.MapFrom(SectionResolver.Resolve<Table, TableDto>))
                .ForMember(x => x.Checklists,x => x.MapFrom(SectionResolver.Resolve<Checklist, ChecklistDto>));

            CreateMap<Checklist, ChecklistDto>()
                .ForMember(x => x.Questions, x => x.MapFrom(ItemResolver.Resolve<Question, QuestionDto>))
                .ForMember(x => x.TextInputs, x => x.MapFrom(ItemResolver.Resolve<TextInput, TextInputDto>))
                .ForMember(x => x.MultipleChoices, x => x.MapFrom(ItemResolver.Resolve<MultipleChoice, MultipleChoiceDto>));

            CreateMap<ProtocolDto, Protocol>()
                .ForMember(x => x.Sections, x => x.MapFrom(SectionResolver.ResolveFromDto));

            CreateMap<ChecklistDto, Checklist>()
                .ForMember(x => x.Items, x => x.MapFrom(ItemResolver.ResolveFromDto));

            CreateMap<TextInput, TextInputDto>().ReverseMap();
            CreateMap<MultipleChoice, MultipleChoiceDto>().ReverseMap();
            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();

            CreateMap<Table, TableDto>().ReverseMap();
            CreateMap<TableRow, TableRowDto>().ReverseMap();
            CreateMap<RowColumn, RowColumnDto>().ReverseMap();
        }
    }
    public static class SectionResolver
    {
        public static List<To> Resolve<From, To>(Protocol source, ProtocolDto destination, List<To> destMember, ResolutionContext context)
            where From : Section
            where To : SectionDto
        {
            return source.Sections.Where(x => x is From).Select(x => x as From).Select(x => context.Mapper.Map<To>(x)).ToList();
        }

        public static List<Section> ResolveFromDto(ProtocolDto source, Protocol destination, List<Section> destMember, ResolutionContext context)
        {
            var sections = new List<Section>();

            var checklists = source.Checklists.Select(x => context.Mapper.Map<Checklist>(x)).ToList();
            var tables = source.Tables.Select(x => context.Mapper.Map<Table>(x)).ToList();

            sections.AddRange(checklists);
            sections.AddRange(tables);

            return sections;
        }
    }

    public static class ItemResolver
    {
        public static List<To> Resolve<From, To>(Checklist source, ChecklistDto destination, List<To> destMember, ResolutionContext context)
            where From : Item
            where To : ItemBaseDto
        {
            return source.Items.Where(x => x is From).Select(x => x as From).Select(x => context.Mapper.Map<To>(x)).ToList();
        }

        public static List<Item> ResolveFromDto(ChecklistDto source, Checklist destination, List<Item> destMember, ResolutionContext context)
        {
            var items = new List<Item>();

            var questions = source.Questions.Select(x => context.Mapper.Map<Question>(x)).ToList();
            var multipleChoices = source.MultipleChoices.Select(x => context.Mapper.Map<MultipleChoice>(x)).ToList();
            var textInputs = source.TextInputs.Select(x => context.Mapper.Map<TextInput>(x)).ToList();

            items.AddRange(questions);
            items.AddRange(multipleChoices);
            items.AddRange(textInputs);

            return items;
        }
    }
}
