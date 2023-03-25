using AutoMapper;
using Traki.Domain.Models.Section;
using Traki.Domain.Models.Section.Items;
using Traki.Domain.Repositories;

namespace Traki.Domain.Handlers
{
    public interface ISectionHandler
    {
        Task AddOrUpdateSection(Section section);
        Task<Section> GetSection(int sectionId);
    }

    public class SectionHandler : ISectionHandler
    {
        private readonly IMapper _mapper;
        private readonly ISectionRepository _sectionRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IItemRepository _itemRepository;

        public SectionHandler(IMapper mapper, ISectionRepository sectionRepository, IChecklistRepository checklistRepository, IItemRepository itemRepository)
        {
            _mapper = mapper;
            _sectionRepository = sectionRepository;
            _checklistRepository = checklistRepository;
            _itemRepository = itemRepository;
        }

        public async Task<Section> GetSection(int sectionId)
        {
            var section = await _sectionRepository.GetSection(sectionId);
            var checklist = await _checklistRepository.GetSectionChecklist(section.Id);
            checklist.Items = (ICollection<Item>)await _itemRepository.GetChecklistItems(checklist.Id);

            section.Checklist = checklist;
            return section;
        }

        public async Task AddOrUpdateSection(Section section)
        {
            var sectionFromDatabase = await _sectionRepository.GetSection(section.Id);

            if (sectionFromDatabase == null)
            {
                sectionFromDatabase = await _sectionRepository.CreateSection(section);
            }
            else
            {
                sectionFromDatabase = await _sectionRepository.UpdateSection(section);
            }


            if (sectionFromDatabase.Checklist != null)
            {
                await DeleteChecklist(sectionFromDatabase.Checklist.Id);
            }
            var checklist = section.Checklist;
            if (checklist != null)
            {
                checklist.SectionId = sectionFromDatabase.Id;
                await CreateChecklist(checklist);
            }
        }

        private async Task DeleteChecklist(int checklistId)
        {
            var checklist = await _checklistRepository.GetChecklist(checklistId);
            if (checklist == null)
            {
                return;
            }

            foreach (var item in checklist.Items)
            {
                await _itemRepository.DeleteItem(item);
            }

            await _checklistRepository.DeleteChecklist(checklist.Id);
        }

        private async Task CreateChecklist(Checklist checklist)
        {

            foreach (var item in checklist.Items)
            {
                item.Id = Guid.NewGuid().ToString();
                if (item.Question != null)
                {
                    item.Question.Id = Guid.NewGuid().ToString();
                }
                if (item.TextInput != null)
                {
                    item.TextInput.Id = Guid.NewGuid().ToString();
                }
                if (item.MultipleChoice != null)
                {
                    item.MultipleChoice.Id = Guid.NewGuid().ToString();
                }
            }

            await _checklistRepository.CreateChecklist(checklist);
        }
    }
}
