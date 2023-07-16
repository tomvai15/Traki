using Traki.Domain.Models.Items;

namespace Traki.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<bool> DeleteItem(Item item);
        Task<Item> CreateItem(Item item);
        Task UpdatedItem(Item item);
        Task<IEnumerable<Item>> GetChecklistItems(int checklistId);
    }
}
