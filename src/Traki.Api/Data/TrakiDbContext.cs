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

        public virtual DbSet<User> Users { get; set; }
    }
}
