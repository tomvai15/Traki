using AutoMapper;
using Traki.Domain.Models.Items;
using Traki.Domain.Models;
using Traki.Infrastructure.Entities.Section.Items;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities;


namespace Traki.Infrastructure.Mapping
{
    public class DomainToInfrastructureMap : Profile
    {
        public DomainToInfrastructureMap()
        {
            MapProtocol();
        }

        public void MapProtocol()
        {
            CreateMap<ChecklistEntity, Checklist>().ForMember(x => x.Items,
                x => x.MapFrom<ItemResolver>());

            CreateMap<ProtocolEntity, Protocol>().ForMember(x => x.Sections,
                x => x.MapFrom<SectionResolver>());

            CreateMap<TextInputEntity, TextInput>();
            CreateMap<MultipleChoiceEntity, MultipleChoice>();
            CreateMap<OptionEntity, Option>();
            CreateMap<QuestionEntity, Question>();

            CreateMap<TableEntity, Table>();
            CreateMap<TableRowEntity, TableRow>();
            CreateMap<RowColumnEntity, RowColumn>();
        }
    }

    public class SectionResolver : IValueResolver<ProtocolEntity, Protocol, List<Section>>
    {
        public List<Section> Resolve(ProtocolEntity source, Protocol destination, List<Section> destMember, ResolutionContext context)
        {
            var sections = new List<Section>();

            var checklists = source.Checklists.Select(x => Map<ChecklistEntity, Checklist>(x, context));
            var tables = source.Tables.Select(x => Map<TableEntity, Table>(x, context));

            sections.AddRange(checklists);
            sections.AddRange(tables);

            return sections;
        }

        private Section Map<From, To>(From section, ResolutionContext context)
            where From : SectionBase
            where To : ISectionContent
        {
            var s = context.Mapper.Map<Section>(section);
            s.SectionContent = context.Mapper.Map<To>(section);
            return s;
        }
    }

    public class ItemResolver : IValueResolver<ChecklistEntity, Checklist, List<Item>>
    {
        public List<Item> Resolve(ChecklistEntity source, Checklist destination, List<Item> destMember, ResolutionContext context)
        {
            var items = new List<Item>();

            var textInputs = source.TextInputs.Select(x => Map<TextInputEntity, TextInput>(x, context));
            var questions = source.Questions.Select(x => Map<QuestionEntity, Question>(x, context));
            var multipleChoices = source.MultipleChoices.Select(x => Map<MultipleChoiceEntity, MultipleChoice>(x, context));

            items.AddRange(textInputs);
            items.AddRange(questions);
            items.AddRange(multipleChoices);

            return items;
        }

        private Item Map<From, To>(From itemEntity, ResolutionContext context)
            where From : ItemBase
            where To : IItemContent
        {
            var item = context.Mapper.Map<Item>(itemEntity);
            item.ItemContent = context.Mapper.Map<To>(itemEntity);
            return item;
        }
    }
}
