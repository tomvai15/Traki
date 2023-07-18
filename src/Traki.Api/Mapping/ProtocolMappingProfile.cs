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
            CreateMap<Protocol, ProtocolDto>().ForMember(x => x.Sections,
                x => x.MapFrom<SectionResolver>());

            CreateMap<Checklist, ChecklistDto>();
            CreateMap<TextInput, TextInputDto>();
            CreateMap<MultipleChoice, MultipleChoiceDto>();
            CreateMap<Option, OptionDto>();
            CreateMap<Question, QuestionDto>();

            CreateMap<Table, TableDto>();
            CreateMap<TableRow, TableRowDto>();
            CreateMap<RowColumn, RowColumnDto>();
        }
    }
    public class SectionResolver : IValueResolver<Protocol, ProtocolDto, ICollection<SectionBaseDto>>
    {
        public ICollection<SectionBaseDto> Resolve(Protocol source, ProtocolDto destination, ICollection<SectionBaseDto> destMember, ResolutionContext context)
        {
            return source.Sections.Select(x => SectionMap(x, context)).ToList();
        }

        private SectionBaseDto SectionMap(Section section, ResolutionContext context)
        {
            if (section is Table table)
            {
                return context.Mapper.Map<TableDto>(table);
            }
            else if (section is Checklist checklist)
            {
                return context.Mapper.Map<ChecklistDto>(checklist);
            }

            throw new ArgumentException($"Type {typeof(Section)} is not regisered");
        }
    }
}
