using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Data;

namespace Traki.UnitTests.Infrastructure.Fixture
{

    public class TrakiDbFixture
    {
        private readonly DbContextOptionsBuilder<TrakiDbContext> contextOptionsBuilder = new DbContextOptionsBuilder<TrakiDbContext>();

        public DbContextOptions<TrakiDbContext> Options { get; private set; }
        public TrakiDbFixture()
        {
            contextOptionsBuilder.UseInMemoryDatabase(databaseName: "TestDb");
            Options = contextOptionsBuilder.Options;

            using var dbContext = new TrakiDbContext(Options);

            dbContext.AddUsers(ExampleData.Users);

            dbContext.AddCompanies();
            dbContext.AddProjects();
            dbContext.AddProducts();
            dbContext.AddQuestions();

            dbContext.AddProtocols();
            dbContext.AddCheclists();
            dbContext.AddNewQuestions();
            dbContext.AddTextInputs();
            dbContext.AddMultipleChoices();

            dbContext.AddDrawings();
            dbContext.AddDefects();
            dbContext.SaveChanges();
        }
    }
}
