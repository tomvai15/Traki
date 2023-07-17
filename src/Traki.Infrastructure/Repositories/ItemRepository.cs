using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Items;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        public Task<Item> CreateItem(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItem(Item item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Item>> GetChecklistItems(int checklistId)
        {
            throw new NotImplementedException();
        }

        public Task UpdatedItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
