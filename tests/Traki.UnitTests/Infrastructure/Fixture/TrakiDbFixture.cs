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

            using var context = new TrakiDbContext(Options);
            context.SaveChanges();
        }
    }
}
