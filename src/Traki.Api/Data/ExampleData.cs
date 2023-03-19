using Traki.Api.Data.Entities;
using Traki.Api.Models;

namespace Traki.Api.Data
{
    public static class ExampleData
    {
        public static IEnumerable<User> Users => new[]
{
            new User
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
                Name = $"Sample Project"
            },
            new ProjectEntity
            {
                Name = $"Other Project"
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

        public static IEnumerable<ChecklistEntity> Checklists => new[]
        {
            new ChecklistEntity
            {
                Name = "Components and Equipment",
                Standard="General components and equipment requirements",
                ProductId = 1
            },
            new ChecklistEntity
            {
                Name = "Test-Checklist-B",
                Standard="ISO-BBD",
                ProductId = 2
            },
            new ChecklistEntity
            {
                Name = "Test-Checklist-C",
                Standard="ISO-BBD",
                ProductId = 2
            }
        };

        public static IEnumerable<QuestionEntity> Questions => new[]
        {
            new QuestionEntity
            {
                Title = "Test-Question-A",
                Description="Test-Description-A",
                TemplateId = 1
            },
            new QuestionEntity
            {
                Title = "Test-Question-B",
                Description="Test-Description-B",
                TemplateId = 1
            },
            new QuestionEntity
            {
                Title = "Test-Question-C",
                Description="Test-Description-C",
                TemplateId = 2
            },            new QuestionEntity
            {
                Title = "Test-Question-D",
                Description="Test-Description-D",
                TemplateId = 1
            },
            new QuestionEntity
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
