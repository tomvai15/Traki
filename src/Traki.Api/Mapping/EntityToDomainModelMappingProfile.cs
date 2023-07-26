using AutoMapper;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Models.Items;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities.Section.Items;
using Question = Traki.Domain.Models.Items.Question;

namespace Traki.Api.Mapping
{
    public class EntityToDomainModelMappingProfile : Profile
    {
        public EntityToDomainModelMappingProfile()
        {
            CreateMap<Drawing, DrawingEntity>().ReverseMap();
            CreateMap<Defect, DefectEntity>().ReverseMap();
            CreateMap<DefectComment, DefectCommentEntity>().ReverseMap();
            CreateMap<StatusChange, StatusChangeEntity>().ReverseMap();
            CreateMap<DefectNotification, DefectNotificationEntity>().ReverseMap();

            CreateMap<Company, CompanyEntity>().ReverseMap();
            CreateMap<User, UserEntity>().ReverseMap();
            CreateMap<Project, ProjectEntity>().ReverseMap();
            CreateMap<Product, ProductEntity>().ReverseMap();
        }
    }
}
