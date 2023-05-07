using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            dbContext.AddTemplates();
            dbContext.AddQuestions();

            dbContext.AddProtocols();
            dbContext.AddSections();
            dbContext.AddCheclists();
            dbContext.AddItems();
            dbContext.AddNewQuestions();
            dbContext.AddTextInputs();
            dbContext.AddMultipleChoices();

            dbContext.AddDrawings();
            dbContext.AddDefects();
            dbContext.SaveChanges();
        }
    }
}
