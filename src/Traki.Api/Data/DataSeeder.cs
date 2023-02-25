using Traki.Api.Models.Project;

namespace Traki.Api.Data
{
    public static class DataSeeder
    {
        public static void AddInitialData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<TrakiDbContext>();

            dbContext.Database.EnsureDeleted();

            if (app.Environment.IsDevelopment())
            {
                dbContext.Database.EnsureDeleted();
            }

            bool wasCreated = dbContext.Database.EnsureCreated();

            dbContext.AddProjects();
        }

        public static TrakiDbContext AddProjects(this TrakiDbContext dbContext)
        {
            dbContext.Projects.AddRange(ExampleData.Projects);
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
