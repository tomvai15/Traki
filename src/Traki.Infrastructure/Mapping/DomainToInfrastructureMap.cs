using AutoMapper;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Models.Items;
using Traki.Domain.Models;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Entities.Section.Items;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities;


namespace Traki.Infrastructure.Mapping
{
    public class DomainToInfrastructureMap : Profile
    {
        public DomainToInfrastructureMap()
        {
            CreateMap<Drawing, DrawingEntity>().ReverseMap();
            CreateMap<Defect, DefectEntity>().ReverseMap();
            CreateMap<DefectComment, DefectCommentEntity>().ReverseMap();
            CreateMap<StatusChange, StatusChangeEntity>().ReverseMap();
            CreateMap<DefectNotification, DefectNotificationEntity>().ReverseMap();
        }

        public void MapProtocol()
        {

            CreateMap<SectionBase, Section>();

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
}
