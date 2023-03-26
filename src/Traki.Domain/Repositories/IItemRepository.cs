using Traki.Domain.Models.Section.Items;

namespace Traki.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<bool> DeleteItem(Item item);
        Task<Item> CreateItem(Item item);
        Task<IEnumerable<Item>> GetChecklistItems(int checklistId);
    }
}
