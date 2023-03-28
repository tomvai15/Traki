using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Data
{
    public static class ExampleData
    {
        public static IEnumerable<DrawingEntity> Drawings => new[]
        {
            new DrawingEntity
            {
                Title= "Drawing 1",
                ImageName = "a8q1bzQ_700b.jpg",
                ProductId = 1
            },
            new DrawingEntity
            {
                Title= "Drawing 2",
                ImageName = "aa4PdXoK_700w_0.jpg",
                ProductId = 1
            }
        };

        public static IEnumerable<DefectEntity> Defects => new[]
        {
            new DefectEntity
            {
                Title  = "Defect 1",
                Description= "Defect 1",
                Status= DefectStatus.NotFixed,
                XPosition = 50,
                YPosition = 50,
                Width = 10,
                Height = 20,
                DrawingId = 1
            },
            new DefectEntity
            {
                Title  = "Defect 2",
                Description= "Defect 2",
                Status= DefectStatus.NotFixed,
                XPosition = 10,
                YPosition = 10,
                Width = 20,
                Height = 10,
                DrawingId = 2
            }
        };


        public static IEnumerable<ProtocolEntity> Protocols => new[]
        {
            new ProtocolEntity
            {
                Name= "Assembly Procedures - Checklist",
                IsTemplate= true,
            },
            new ProtocolEntity
            {
                Name= "Welding Procedures - Checklist",
                IsTemplate= true,
            }
        };

        public static IEnumerable<SectionEntity> Sections => new[]
        {
            new SectionEntity
            {
                Name = "test",
                Priority = 1,
                ProtocolId = 1,
            }
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
                Email = "vainoristomas@gmail.com",
                HashedPassword="$2a$11$fwHqYCKHBg5nOiswpHmQ5eBn1er5kr6DyDCPa7BOndAJBM6IDjTHa",
                Role = "admin"
            }
        };

        public static IEnumerable<ProjectEntity> Projects => new[]
        {
            new ProjectEntity
            {
                Name = $"Sample Project",
                CompanyId = 1,
            },
            new ProjectEntity
            {
                Name = $"Other Project",
                CompanyId = 1,
            }
        };

        public static IEnumerable<ProductEntity> Products => new[]
        {
            new ProductEntity
            {
                Name = $"SH.1 / 01.2.21.1.0016",
                ProjectId = 1
            },
            new ProductEntity
            {
                Name = $"SH.2 / 01.2.21.1.0016 GT",
                ProjectId = 2
            }
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
