using Microsoft.EntityFrameworkCore;
using Traki.Api.Models;

namespace Traki.Api.Data
{
    public class TrakiDbContext : DbContext
    {
        public TrakiDbContext(DbContextOptions<TrakiDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
