using AutoMapper;
using Traki.Domain.Extensions;
using Traki.Domain.Models.Section;
using Traki.Domain.Models.Section.Items;
using Traki.Domain.Repositories;

namespace Traki.Domain.Handlers
{
    public interface ISectionHandler
    {
        Task UpdateSections(IEnumerable<Section> sections);
        Task AddOrUpdateSection(int protocolId, Section section);
        Task<Section> GetSection(int sectionId);
        Task DeleteSection(int sectionId);
        Task<IEnumerable<Section>> GetSections(int protocolId);
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

        public async Task UpdateSections(IEnumerable<Section> sections)
        {
            foreach (var section in sections)
            {
                await _sectionRepository.UpdateSection(section);
            }
        }

        public async Task<Section> GetSection(int sectionId)
        {
            var section = await _sectionRepository.GetSection(sectionId);
            var checklist = await _checklistRepository.GetSectionChecklist(section.Id);
            if (checklist != null)
            {
                checklist.Items = (ICollection<Item>)await _itemRepository.GetChecklistItems(checklist.Id);
            }

            section.Checklist = checklist;
            return section;
        }

        public async Task AddOrUpdateSection(int protocolId, Section section)
        {
            var sectionFromDatabase = await _sectionRepository.GetSection(section.Id);

            int priority = (await _sectionRepository.GetSections(section.Id)).Count() + 1;

            if (sectionFromDatabase == null)
            {
                section.Priority = priority;
                sectionFromDatabase = await _sectionRepository.CreateSection(section);
            }
            else
            {
                sectionFromDatabase = await _sectionRepository.UpdateSection(section);
            }

            var checklist = await _checklistRepository.GetSectionChecklist(section.Id);
            if (checklist != null)
            {
                await DeleteChecklist(checklist.Id);
            }
            checklist = section.Checklist;
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

        public async Task<IEnumerable<Section>> GetSections(int protocolId)
        {
            return await _sectionRepository.GetSections(protocolId);
        }

        public async Task DeleteSection(int sectionId)
        {
            var sectionFromDatabase = await _sectionRepository.GetSection(sectionId);

            sectionFromDatabase.RequiresToBeNotNullEnity();

            var checklist = await _checklistRepository.GetSectionChecklist(sectionId);
            if (checklist != null)
            {
                await DeleteChecklist(checklist.Id);
            }

            await _sectionRepository.DeleteSection(sectionFromDatabase);
        }
    }
}
