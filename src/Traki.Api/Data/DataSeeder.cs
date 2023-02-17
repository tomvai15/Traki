namespace Traki.Api.Data
{
    public static class DataSeeder
    {
        public static void AddInitialData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<TrakiDbContext>();
            bool wasCreated = dbContext.Database.EnsureCreated();
        }
    }
}
