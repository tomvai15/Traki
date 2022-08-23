using Inventory.Infrastructure.Parts;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
           : base(options)
        {
        }
        public DbSet<PartDto> Parts { get; set; }
    }
}
