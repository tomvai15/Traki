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
            CreateMap<Protocol, ProtocolDto>().ReverseMap();
            CreateMap<Checklist, ChecklistDto>().ReverseMap();

            CreateMap<TextInput, TextInputDto>().ReverseMap();
            CreateMap<MultipleChoice, MultipleChoiceDto>().ReverseMap();
            CreateMap<Option, OptionDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();

            CreateMap<Table, TableDto>().ReverseMap();
            CreateMap<TableRow, TableRowDto>().ReverseMap();
            CreateMap<RowColumn, RowColumnDto>().ReverseMap();

            CreateMap<Item, ItemDto>()
                .ForMember(x => x.TextInput, x => x.MapFrom(x => x.ItemContent is TextInput ? x.ItemContent as TextInput : null))
                .ForMember(x => x.MultipleChoice, x => x.MapFrom(x => x.ItemContent is MultipleChoice ? x.ItemContent as MultipleChoice : null))
                .ForMember(x => x.Question, x => x.MapFrom(x => x.ItemContent is Question ? x.ItemContent as Question : null));

            CreateMap<Section, SectionDto>()
                .ForMember(x => x.Checklist, x => x.MapFrom(x => x.SectionContent is Checklist ? x.SectionContent as Checklist : null))
                .ForMember(x => x.Table, x => x.MapFrom(x => x.SectionContent is Table ? x.SectionContent as Table : null));


            CreateMap<ItemDto, Item>()
                .ForMember(x => x.ItemContent, x => x.MapFrom(MapFromItem));

            CreateMap<SectionDto, Section>()
                .ForMember(x => x.SectionContent, x => x.MapFrom(MapFromSection));
        }

        private IItemContent MapFromItem(ItemDto source, Item destination, IItemContent destMember, ResolutionContext context)
        {
            if (source.Question != null)
            {
                return context.Mapper.Map<Question>(source.Question);
            }
            if (source.TextInput != null)
            {
                return context.Mapper.Map<TextInput>(source.TextInput);
            }
            if (source.MultipleChoice != null)
            {
                return context.Mapper.Map<MultipleChoice>(source.MultipleChoice);
            }
            return null;
        }

        private ISectionContent MapFromSection(SectionDto source, Section destination, ISectionContent destMember, ResolutionContext context)
        {
            if (source.Checklist != null)
            {
                return context.Mapper.Map<Checklist>(source.Checklist);
            }
            if (source.Table != null)
            {
                return context.Mapper.Map<Table>(source.Table);
            }
            return null;
        }
    }
}
