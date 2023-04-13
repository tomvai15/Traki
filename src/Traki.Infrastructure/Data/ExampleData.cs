using Traki.Domain.Constants;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Models.Section.Items;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Data
{
    public static class ExampleData
    {
        public static IEnumerable<DrawingEntity> Drawings => new[]
        {
            new DrawingEntity
            {
                Title= "Drawing 1",
                ImageName = "back.png",
                ProductId = 1
            },
            new DrawingEntity
            {
                Title= "Drawing 2",
                ImageName = "front.png",
                ProductId = 1
            },
            new DrawingEntity
            {
                Title= "Drawing 1",
                ImageName = "front.png",
                ProductId = 2
            }
        };

        public static IEnumerable<DefectEntity> Defects => new[]
        {
            new DefectEntity
            {
                Title  = "Defect 1",
                Description= "Defect 1",
                ImageName = "ce388de4-8c6b-45b0-ab12-85f78cfa900c.png",
                Status= DefectStatus.NotFixed,
                X = 50,
                Y = 50,
                Width = 10,
                Height = 20,
                DrawingId = 1,
                UserId = 1,
            },
            new DefectEntity
            {
                Title  = "Defect 2",
                Description= "Defect 2",
                ImageName = "99696138-0e26-4815-9982-40b575269336.png",
                Status= DefectStatus.NotFixed,
                X = 10,
                Y = 10,
                Width = 20,
                Height = 10,
                DrawingId = 2,
                UserId = 1,
            }
        };


        public static IEnumerable<ProtocolEntity> Protocols => new[]
        {
            new ProtocolEntity
            {
                Name= "Welding Procedures - Checklist",
                IsTemplate= true,
            },
            new ProtocolEntity
            {
                Name= "Leakage test - Protocol",
                IsTemplate= true,
            }
        };

        public static IEnumerable<SectionEntity> Sections => new[]
        {
            new SectionEntity
            {
                Name= "General Check-up BEFORE & DURING welding [ISO 13480-5]",
                Priority = 1,
                ProtocolId= 1,
            },
            new SectionEntity
            {
                Name= "General Check-up AFTER welding [ISO 13480-5]",
                Priority = 2,
                ProtocolId= 1,
            },
            new SectionEntity
            {
                Name= "General check up before Leakage test",
                Priority = 1,
                ProtocolId= 2,
            },
            new SectionEntity
            {
                Name= "Test procedures",
                Priority = 2,
                ProtocolId= 2,
            },
        };

        public static IEnumerable<ChecklistEntity> NewChecklists => new[]
        {
            new ChecklistEntity
            {
                SectionId = 1,
            },
            new ChecklistEntity
            {
                SectionId = 2,
            },
            new ChecklistEntity
            {
                SectionId = 3,
            },
            new ChecklistEntity
            {
                SectionId = 4,
            },
        };

        public static IEnumerable<ItemEntity> Items => new[]
        {
            new ItemEntity
            {
                Id = "A",
                Name = "Is the WPS clear for welding procedures?",
                ChecklistId = 1,
                Priority = 1,
            },
            new ItemEntity
            {
                Id = "B",
                Name = "Do the general prefab dimensions meet the design?",
                ChecklistId = 1,
                Priority = 2,
            },

            new ItemEntity
            {
                Id = "C",
                Name = "Are all valves and pipes plugged?",
                ChecklistId = 3,
                Priority = 1,
            },
            new ItemEntity
            {
                Id = "D",
                Name = "Are safety barries applied and warning signs hung up?",
                ChecklistId = 3,
                Priority = 2,
            },

            new ItemEntity
            {
                Id = "E",
                Name = "Manometer ID, Serial No",
                ChecklistId = 4,
                Priority = 1,
            },
            new ItemEntity
            {
                Id = "F",
                Name = "Test type",
                ChecklistId = 4,
                Priority = 2,
            },
        };

        public static IEnumerable<QuestionEntity> NewQuestions => new[]
        {
            new QuestionEntity
            {
                 Id = "A",
                 ItemId = "A",
                 Comment = "",
            },
            new QuestionEntity
            {
                Id = "B",
                ItemId = "B",
                Comment = "",
            },

            new QuestionEntity
            {
                 Id = "C",
                 ItemId = "C",
                 Comment = "",
            },
            new QuestionEntity
            {
                Id = "D",
                ItemId = "D",
                Comment = "",
            },
        };

        public static IEnumerable<TextInputEntity> TextInputs => new[]
{
            new TextInputEntity
            {
                 Id = "E",
                 ItemId = "E",
                 Value= ""
            },
        };

        public static IEnumerable<MultipleChoiceEntity> MultipleChoices => new[]
{
            new MultipleChoiceEntity
            {
                 Id = "F",
                 ItemId = "F",
                 Options = new OptionEntity[]
                 {
                    new OptionEntity
                    {
                         Id = "G",
                         Name = "Pneumatic",
                    },
                    new OptionEntity
                    {
                        Id = "H",
                        Name = "Hydrostatic",
                    },
                 }
            },
        };

        public static IEnumerable<CompanyEntity> Companies => new[]
        {
            new CompanyEntity
            {
                Name = "Preftek",
                Address = "Vydūno alėja",
                CreationDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                ImageName = "Preftek-full-logo.png"
            }
        };

        public static IEnumerable<UserEntity> Users => new[]
        {
            new UserEntity
            {
                RegisterId = "abc",
                Email = "vainoristomas@gmail.com",
                Name = "Tomas",
                Surname = "Vainoris",
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",
                Role = Role.ProductManager,
                Status = UserStatus.Active
            },
            new UserEntity
            {
                Email = "vainoristomas9@gmail.com",
                Name = "Domas",
                Surname = "Bainoris",
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",
                Role = Role.ProjectManager,
                Status = UserStatus.Active
            },

            /*
            new UserEntity
            {
                Email = "tipotomas9@gmail.com",
                Name = "Romas",
                Surname = "Fainoris",
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",
                Role = Role.Administrator,
                Status = UserStatus.Active
            }*/
        };

        public static IEnumerable<ProjectEntity> Projects => new[]
        {
            new ProjectEntity
            {
                Name = $"Sample Project",
                ClientName = $"Sample Client",
                Address = $"Sample Address",
                ImageName = "5e6669e4-c033-4ded-93e6-f44dccc6a157.png",
                CompanyId = 1,
                UserId = 2,
            },
            new ProjectEntity
            {
                Name = $"Other Project",
                ClientName = $"Sample Client",
                Address = $"Sample Address",
                ImageName = "02653609-3004-4a48-b2d6-bddab03e778c.png",
                CompanyId = 1,
                UserId = 2,
            }
        };

        public static IEnumerable<ProductEntity> Products => new[]
        {
            new ProductEntity
            {
                Name = $"SH.1 / 01.2.21.1.0016",
                ProjectId = 1,
                UserId = 1
            },
            new ProductEntity
            {
                Name = $"SH.2 / 01.2.21.1.0016 GT",
                ProjectId = 2,
                UserId = 1
            },
            new ProductEntity
            {
                Name = $"SH.1 / 01.2.21.1.0016",
                ProjectId = 1,
                UserId = 1
            },
        };

        public static IEnumerable<TemplateEntity> Templates => new[]
        {
            new TemplateEntity
            {
                Name = "General",
                Standard="General prefab requirements",
                ProjectId = 1
            },
            new TemplateEntity
            {
                Name = "Piping",
                Standard="General piping requirements",
                ProjectId = 1
            },
            new TemplateEntity
            {
                Name = "Components and Equipment",
                Standard="General components and equipment requirements",
                ProjectId = 1
            },
            new TemplateEntity
            {
                Name = "General Check-up Before & During Welding",
                Standard="ISO 13480-5 standard",
                ProjectId = 2
            }
        };

        public static IEnumerable<OldChecklistEntity> Checklists => new[]
        {
            new OldChecklistEntity
            {
                Name = "Components and Equipment",
                Standard="General components and equipment requirements",
                ProductId = 1
            },
            new OldChecklistEntity
            {
                Name = "Test-Checklist-B",
                Standard="ISO-BBD",
                ProductId = 2
            },
            new OldChecklistEntity
            {
                Name = "Test-Checklist-C",
                Standard="ISO-BBD",
                ProductId = 2
            }
        };

        public static IEnumerable<OldQuestionEntity> Questions => new[]
        {
            new OldQuestionEntity
            {
                Title = "Test-Question-A",
                Description="Test-Description-A",
                TemplateId = 1
            },
            new OldQuestionEntity
            {
                Title = "Test-Question-B",
                Description="Test-Description-B",
                TemplateId = 1
            },
            new OldQuestionEntity
            {
                Title = "Test-Question-C",
                Description="Test-Description-C",
                TemplateId = 2
            },            new OldQuestionEntity
            {
                Title = "Test-Question-D",
                Description="Test-Description-D",
                TemplateId = 1
            },
            new OldQuestionEntity
            {
                Title = "Test-Question-E",
                Description="Test-Description-E",
                TemplateId = 1
            },
        };

        public static IEnumerable<ChecklistQuestionEntity> CheckListQuestions => new[]
        {
            new ChecklistQuestionEntity
            {
                Title = "Test-ChecklistQuestion-A",
                Description="Test-Description-A",
                ChecklistId = 1
            },
            new ChecklistQuestionEntity
            {
                Title = "Test-ChecklistQuestion-B",
                Description="Test-Description-B",
                ChecklistId = 1
            },
            new ChecklistQuestionEntity
            {
                Title = "Test-ChecklistQuestion-C",
                Description="Test-Description-C",
                ChecklistId = 2
            },
            new ChecklistQuestionEntity
            {
                Title = "Test-ChecklistQuestion-D",
                Description="Test-Description-D",
                ChecklistId = 1
            },
            new ChecklistQuestionEntity
            {
                Title = "Test-ChecklistQuestion-E",
                Description="Test-Description-E",
                ChecklistId = 1
            },
        };
    }
}
