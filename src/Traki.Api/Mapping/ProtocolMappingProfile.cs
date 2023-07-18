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
            CreateMap<Protocol, ProtocolDto>().ForMember(x => x.Tables,
                x => x.MapFrom(SectionResolver.Resolve<Table, TableDto>))
                .ForMember(x => x.Checklists,
                x => x.MapFrom(SectionResolver.Resolve<Checklist, ChecklistDto>));

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
    public static class SectionResolver
    {
        public static List<To> Resolve<From, To>(Protocol source, ProtocolDto destination, List<To> destMember, ResolutionContext context)
            where From : Section
            where To : SectionBaseDto
        {
            return source.Sections.Where(x => x is From).Select(x => x as From).Select(x => context.Mapper.Map<To>(x)).ToList();
        }
    }
}
