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

            CreateMap<ProtocolEntity, Protocol>().ForMember(x => x.Sections,
                x => x.MapFrom<SectionResolver>());

            CreateMap<ChecklistEntity, Checklist>().ForMember(x => x.Items,
                x => x.MapFrom<ItemResolver>());

            CreateMap<ChecklistEntity, Checklist>();
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

            var checklists =  context.Mapper.Map<IEnumerable<Checklist>>(source.Checklists);
            var tables = context.Mapper.Map<IEnumerable<Table>>(source.Tables);

            sections.AddRange(checklists);
            sections.AddRange(tables);

            return sections;
        }
    }

    public class ItemResolver : IValueResolver<ChecklistEntity, Checklist, List<Item>>
    {
        public List<Item> Resolve(ChecklistEntity source, Checklist destination, List<Item> destMember, ResolutionContext context)
        {
            var items = new List<Item>();

            var textInputs = context.Mapper.Map<IEnumerable<TextInput>>(source.TextInputs);
            var questions = context.Mapper.Map<IEnumerable<Question>>(source.Questions);
            var multipleChoices = context.Mapper.Map<IEnumerable<MultipleChoice>>(source.MultipleChoices);

            items.AddRange(textInputs);
            items.AddRange(questions);
            items.AddRange(multipleChoices);

            return items;
        }
    }
}
