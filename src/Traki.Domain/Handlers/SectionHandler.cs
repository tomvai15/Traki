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
        Task UpdateSectionAnswers(int protocolId, Section section);
        Task<Section> GetSection(int sectionId);
        Task DeleteSection(int sectionId);
        Task<IEnumerable<Section>> GetSections(int protocolId);
    }

    public class SectionHandler : ISectionHandler
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IItemRepository _itemRepository;
        private readonly ITableRepository _tableRepository;
        private readonly ITableRowRepository _tableRowRepository;

        public SectionHandler(ISectionRepository sectionRepository, 
            IChecklistRepository checklistRepository, 
            IItemRepository itemRepository, 
            ITableRepository tableRepository,
            ITableRowRepository tableRowRepository)
        {
            _sectionRepository = sectionRepository;
            _checklistRepository = checklistRepository;
            _itemRepository = itemRepository;
            _tableRepository = tableRepository;
            _tableRowRepository = tableRowRepository;
        }

        public async Task<Section> GetSection(int sectionId)
        {
            var section = await _sectionRepository.GetSection(sectionId);
            var checklist = await _checklistRepository.GetSectionChecklist(section.Id);
            if (checklist != null)
            {
                var items = await _itemRepository.GetChecklistItems(checklist.Id);
                checklist.Items = items.OrderBy(x => x.Priority).ToList();
            }
            section.Checklist = checklist;

            var table = await _tableRepository.GetSectionTable(section.Id);
            if (table != null)
            {
                
            }
            section.Table = table;
            return section;
        }

        public async Task UpdateSections(IEnumerable<Section> sections)
        {
            foreach (var section in sections)
            {
                await _sectionRepository.UpdateSection(section);
            }
        }

        public async Task UpdateSectionAnswers(int protocolId, Section section)
        {
            var checklistToUpdate =  await _checklistRepository.GetSectionChecklist(section.Id);
            if (checklistToUpdate != null)
            {
                checklistToUpdate = await _checklistRepository.GetChecklist(checklistToUpdate.Id);
                var checklist = section.Checklist;
                if (checklistToUpdate != null && checklist != null)
                {
                    await UpdateChecklistModel(checklist, checklistToUpdate);
                }
            }
        }

        public async Task AddOrUpdateSection(int protocolId, Section section)
        {
            section.ProtocolId = protocolId;
            var sectionFromDatabase = await _sectionRepository.GetSection(section.Id);

            int priority = (await _sectionRepository.GetSections(protocolId)).Count() + 1;

            if (sectionFromDatabase == null)
            {
                section.Priority = priority;
                sectionFromDatabase = await _sectionRepository.CreateSection(section);
            }
            else
            {
                sectionFromDatabase = await _sectionRepository.UpdateSection(section);
            }

            await UpdateChecklist(section, sectionFromDatabase.Id);
            await UpdateTable(section, sectionFromDatabase.Id);
        }

        private async Task UpdateTable(Section section, int sectionId)
        {
            var table = await _tableRepository.GetSectionTable(section.Id);
            if (table != null)
            {
                await _tableRepository.DeleteTable(table.Id);
            }

            if (section.Table == null)
            {
                return;
            }
            table = section.Table;
            table.Id = 0;
            table.SectionId = sectionId;
            table.TableRows = section.Table.TableRows;
            await CreateTable(table);   
        }

        private async Task CreateTable(Table table)
        {
            var tableRows = table.TableRows;
            table.TableRows = null;
            table = await _tableRepository.CreateTable(table);
            foreach (var tableRow in tableRows) 
            {
                tableRow.TableId = table.Id;
                await _tableRowRepository.CreateTableRow(tableRow);
            }
            return;
        }

        private async Task UpdateChecklist(Section section, int sectionId)
        {
            var checklist = await _checklistRepository.GetSectionChecklist(section.Id);
            if (checklist != null)
            {
                await DeleteChecklist(checklist.Id);
            }
            if (section.Checklist == null)
            {
                return;
            }
            checklist = section.Checklist;
            if (checklist != null)
            {
                checklist.SectionId = sectionId;
                await CreateChecklist(checklist);
            }
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

        private async Task UpdateChecklistModel(Checklist checklist, Checklist checklistToUpdate)
        {
            foreach (var itemToUpdate in checklistToUpdate.Items)
            {
                var item = checklist.Items.First(x => x.Id == itemToUpdate.Id);

                await _itemRepository.UpdatedItem(item);
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
                UpdateItemIds(item);
            }

            await _checklistRepository.CreateChecklist(checklist);
        }

        private void UpdateItemIds(Item item)
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
                foreach (var option in item.MultipleChoice.Options)
                {
                    option.Id = Guid.NewGuid().ToString();
                }
            }
        }
    }
}
