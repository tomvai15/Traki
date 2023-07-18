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

        public static IEnumerable<ChecklistEntity> Checklists => new[]
        {
            new ChecklistEntity
            {
                Name = "Checklist",
                ProtocolId = 1
            },
        };

        public static IEnumerable<TableEntity> Tables => new[]
        {
            new TableEntity
            {
                Name = "Table",
                ProtocolId = 1
            },
        };

        public static IEnumerable<TableRowEntity> TableRows => new[]
{
            new TableRowEntity
            {
                RowIndex = 0,
                TableId = 1
            },
        };

        public static IEnumerable<RowColumnEntity> RowColumns => new[]
{
            new RowColumnEntity
            {
                Value = "Column",
                TableRowId = 1
            },
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

        public static IEnumerable<QuestionEntity> NewQuestions => new[]
        {
            new QuestionEntity
            {
                Name = "Is the WPS clear for welding procedures?",
                ChecklistId = 1,
                Index = 1,
                Comment = "Test Comment",
            }
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
    }
}
