using Traki.Api.Entities;

namespace Traki.Api.Data
{
    public static class ExampleData
    {
        public static IEnumerable<ProjectEntity> Projects => new[]
        {
            new ProjectEntity
            {
                Name = $"Test-Project-A"
            },
            new ProjectEntity
            {
                Name = $"Test-Project-B"
            }
        };

        public static IEnumerable<ProductEntity> Products => new[]
        {
            new ProductEntity
            {
                Name = $"Test-Product-A",
                ProjectId = 1
            },
            new ProductEntity
            {
                Name = $"Test-Product-B",
                ProjectId = 2
            }
        };

        public static IEnumerable<TemplateEntity> Templates => new[]
        {
            new TemplateEntity
            {
                Name = "Test-Templatet-A",
                Standard="ISO-BBD",
                ProjectId = 1
            },
            new TemplateEntity
            {
                Name = "Test-Templatet-B",
                Standard="ISO-BBD",
                ProjectId = 1
            },
            new TemplateEntity
            {
                Name = "Test-Templatet-C",
                Standard="ISO-BBD",
                ProjectId = 2
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
            }
        };
    }
}
