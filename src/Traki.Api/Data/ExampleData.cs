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
    }
}
