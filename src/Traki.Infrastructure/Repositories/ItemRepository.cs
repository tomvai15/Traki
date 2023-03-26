using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Domain.Models.Section.Items;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ItemRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Item> CreateItem(Item item)
        {
            var itemEntity = _mapper.Map<ItemEntity>(item);
            itemEntity = (_context.Items.Add(itemEntity)).Entity;
            _context.SaveChanges();

            return _mapper.Map<Item>(itemEntity);
        }

        public async Task<bool> DeleteItem(Item item)
        {
            var textInput = _context.TextInputs.Where(x=> x.ItemId == item.Id);
            var question = _context.Questions.Where(x => x.ItemId == item.Id);
            var multipleChoice = _context.MultipleChoices
                                            .Where(x => x.ItemId == item.Id)
                                            .Include(x=> x.Options);


            _context.TextInputs.RemoveRange(textInput);
            _context.Questions.RemoveRange(question);
            _context.MultipleChoices.RemoveRange(multipleChoice);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Item>> GetChecklistItems(int checklistId)
        {
            var items = await _context.Items.Where(x => x.ChecklistId== checklistId)
                .Include(x=> x.TextInput)
                .Include(x=> x.Question)
                .Include(x => x.MultipleChoice).ThenInclude(x => x.Options)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Item>>(items);
        }
    }
}
