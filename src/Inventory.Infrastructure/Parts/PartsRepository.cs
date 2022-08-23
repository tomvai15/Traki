using Inventory.Infrastructure.Data;
using Microsoft;

namespace Inventory.Infrastructure.Parts
{
    public interface IPartsRepository
    {
        public Task<IEnumerable<PartDto>> GetParts();
    }
    public class PartsRepository : IPartsRepository
    {
        private readonly InventoryDbContext inventoryDbContext;

        public PartsRepository(InventoryDbContext inventoryDbContext)
        {
            Requires.NotNull(inventoryDbContext, nameof(inventoryDbContext));

            this.inventoryDbContext = inventoryDbContext;
            inventoryDbContext.Database.EnsureCreated();
        }

        public async Task<IEnumerable<PartDto>> GetParts()
        {
            return inventoryDbContext.Parts.ToList();
        }
    }
}
