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

        public async Task UpdatedItem(Item item)
        {
            var itemEntity = _context.Items.Where(x => x.Id == item.Id)
                                .Include(x => x.TextInput)
                                .Include(x => x.Question)
                                .Include(x => x.MultipleChoice).ThenInclude(x => x.Options)
                                .FirstOrDefault();

            if (itemEntity == null)
            {
                return;
            }

            await UpdateItemAnswers(itemEntity, item);

            itemEntity = (_context.Items.Add(itemEntity)).Entity;
            _context.SaveChanges();
        }

        public async Task<bool> DeleteItem(Item item)
        {
            var textInput = _context.TextInputs.Where(x => x.ItemId == item.Id);
            var question = _context.Questions.Where(x => x.ItemId == item.Id);
            var multipleChoice = _context.MultipleChoices
                                            .Where(x => x.ItemId == item.Id)
                                            .Include(x => x.Options);


            _context.TextInputs.RemoveRange(textInput);
            _context.Questions.RemoveRange(question);
            _context.MultipleChoices.RemoveRange(multipleChoice);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Item>> GetChecklistItems(int checklistId)
        {
            var items = await _context.Items.Where(x => x.ChecklistId == checklistId)
                .Include(x => x.TextInput)
                .Include(x => x.Question)
                .Include(x => x.MultipleChoice).ThenInclude(x => x.Options)
                .ToListAsync();

            return _mapper.Map<IEnumerable<Item>>(items);
        }

        private async Task UpdateItemAnswers(ItemEntity itemToUpdate, Item item)
        {
            if (itemToUpdate == null)
            {
                return;
            }

            if (itemToUpdate.Question != null && item.Question != null)
            {
                itemToUpdate.Question.Comment = item.Question.Comment;
                itemToUpdate.Question.Answer = item.Question.Answer;
            }
            else if (itemToUpdate.TextInput != null && item.TextInput != null)
            {
                itemToUpdate.TextInput.Value = item.TextInput.Value;
            }
            else if (itemToUpdate.MultipleChoice != null && item.MultipleChoice != null)
            {
                foreach (var optionToUpdate in itemToUpdate.MultipleChoice.Options)
                {
                    var option = item.MultipleChoice.Options.FirstOrDefault(x => x.Id == optionToUpdate.Id);
                    if (option != null)
                    {
                        optionToUpdate.Selected = option.Selected;
                    }
                }
            }
        }
    }
}
