using Traki.Domain.Constants;
using Traki.Domain.Models.Drawing;
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
                Title= "Front view",
                ImageName = "back.png",
                ProductId = 1
            },
            new DrawingEntity
            {
                Title= "Side view",
                ImageName = "front.png",
                ProductId = 1
            },
            new DrawingEntity
            {
                Title= "Front view",
                ImageName = "naujas.png",
                ProductId = 2
            },
            new DrawingEntity
            {
                Title= "Front view",
                ImageName = "kitas.png",
                ProductId = 3
            }
        };

        public static IEnumerable<DefectEntity> Defects => new[]
        {
            new DefectEntity
            {
                Title  = "Bad weld",
                Description= "There are cracks in the weld",
                ImageName = "ce388de4-8c6b-45b0-ab12-85f78cfa900c.png",
                Status= DefectStatus.NotFixed,
                X = 50,
                Y = 50,
                Width = 10,
                Height = 10,
                DrawingId = 1,
                AuthorId = 1,
                CreationDate = DateTime.Now
            },
            new DefectEntity
            {
                Title  = "Incorrectly drilled threads",
                Description= "Thread diameter should be 20mm",
                ImageName = "hole.png",
                Status= DefectStatus.NotFixed,
                X = 50,
                Y = 30,
                Width = 20,
                Height = 10,
                DrawingId = 2,
                AuthorId = 1,
                CreationDate = DateTime.Now
            }
        };


        public static IEnumerable<ProtocolEntity> Protocols => new[]
        {
            new ProtocolEntity
            {
                Name= "Welding Procedures - Checklist",
                IsTemplate= true,
                CreationDate = DateTime.Now.ToString("s"),
            },
            new ProtocolEntity
            {
                Name= "Leakage test - Protocol",
                IsTemplate= true,
                CreationDate = DateTime.Now.ToString("s"),
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
                Id = "X",
                Name = "Is the weld root welded completely?",
                ChecklistId = 2,
                Priority = 1,
            },
            new ItemEntity
            {
                Id = "Y",
                Name = "Is the inner weld surface smooth?",
                ChecklistId = 2,
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

            new QuestionEntity
            {
                 Id = "X",
                 ItemId = "X",
                 Comment = "",
            },
            new QuestionEntity
            {
                Id = "Y",
                ItemId = "Y",
                Comment = "",
            },
        };

        public static IEnumerable<TextInputEntity> TextInputs => new TextInputEntity[0];

        public static IEnumerable<MultipleChoiceEntity> MultipleChoices => new MultipleChoiceEntity[0];

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
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",// password
                Role = Role.ProductManager,
                Status = UserStatus.Active,
            },
            new UserEntity
            {
                Email = "vainoristomas9@gmail.com",
                Name = "Domas",
                Surname = "Bainoris",
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",// password
                Role = Role.ProjectManager,
                Status = UserStatus.Active
            },         
            new UserEntity
            {
                Email = "admin@gmail.com",
                Name = "Romas",
                Surname = "Fainoris",
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",// password
                Role = Role.Administrator,
                Status = UserStatus.Active
            }
        };

        public static IEnumerable<ProjectEntity> Projects => new[]
        {
            new ProjectEntity
            {
                Name = $"Shunt group project",
                ClientName = $"UAB Ever Green",
                Address = $"Vydūno alėja 24A, Kaunas",
                ImageName = "5e6669e4-c033-4ded-93e6-f44dccc6a157.png",
                CreationDate = DateTime.Now.ToString("s"),
                CompanyId = 1,
                AuthorId = 2,
            },
            new ProjectEntity
            {
                Name = $"Pump kit",
                ClientName = $"UAB Star platinum",
                Address = $"Vydūno alėja 24A, Kaunas",
                ImageName = "test.png",
                CompanyId = 1,
                AuthorId = 2,
                CreationDate = DateTime.Now.ToString("s"),
            }
        };

        public static IEnumerable<ProductEntity> Products => new[]
        {
            new ProductEntity
            {
                Name = $"SH.1 / 01.2.21.1.0016",
                ProjectId = 1,
                AuthorId = 1,
                Status = ProductStatus.Active,
                CreationDate = DateTime.Now.ToString("s"),
            },
            new ProductEntity
            {
                Name = $"SH.2",
                ProjectId = 2,
                AuthorId = 1,
                Status = ProductStatus.Completed,
                CreationDate = DateTime.Now.ToString("s"),
            },
            new ProductEntity
            {
                Name = $"TH.1 - 0016",
                ProjectId = 1,
                AuthorId = 1,
                Status = ProductStatus.Active,
                CreationDate = DateTime.Now.ToString("s"),
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
    }
}
