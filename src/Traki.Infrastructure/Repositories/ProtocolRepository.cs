using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Repositories
{
    public class ProtocolRepository : IProtocolRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ProtocolRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Protocol> CreateProtocol(Protocol protocol)
        {
            var protocolEntity = _mapper.Map<ProtocolEntity>(protocol);
            _context.Add(protocolEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<Protocol>(protocolEntity);
        }

        public Task DeleteProtocol(int protocolId)
        {
            throw new NotImplementedException();
        }

        public async Task<Protocol> GetProtocol(int protocolId)
        {
            var protocol = _context.Protocols
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.TextInput)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.Question)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Checklist)
                                            .ThenInclude(x => x.Items)
                                                .ThenInclude(x => x.MultipleChoice)
                                                    .ThenInclude(x => x.Options)
                                    .Include(x => x.Sections)
                                        .ThenInclude(x => x.Table)
                                            .ThenInclude(x => x.TableRows)
                                                .ThenInclude(x => x.RowColumns)
                                    .First(x => x.Id == protocolId);

            var a = _mapper.Map<Protocol>(protocol);

            return a;
        }

        public Task<IEnumerable<Protocol>> GetProtocols(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Protocol>> GetTemplateProtocols()
        {
            var protocols = await _context.Protocols.Where(p => p.IsTemplate == true).ToListAsync();

            return _mapper.Map<IEnumerable<Protocol>>(protocols);
        }

        public async Task<Protocol> UpdateProtocol(Protocol protocol)
        {
            var protocolEntity = _mapper.Map<ProtocolEntity>(protocol);

            var sections = protocolEntity.Sections;

            protocolEntity.Sections = null;

            var initialProtocolSections = _context.Sections.Where(s => s.ProtocolId == protocol.Id).ToList();

            var newSections = sections.Where(x => x.Id == 0).ToList();
            var oldSections = sections.Where(x => x.Id != 0).ToList();
            var protocolsToDelete = initialProtocolSections.ExceptBy(oldSections.Select(x => x.Id), x => x.Id).ToList();

            UpdateSections(oldSections);
            _context.Sections.RemoveRange(protocolsToDelete);
            _context.Sections.AddRange(newSections);

            _context.Protocols.Update(protocolEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<Protocol>(protocolEntity);
        }
        
        private void UpdateSections(IEnumerable<SectionEntity> sections)
        {
            foreach (var section in sections)
            {
                UpdateChecklist(section.Checklist);
             //   UpdateTable(section.Table);  
                section.Table = null;
                section.Checklist = null;
            }

            _context.Sections.UpdateRange(sections);
        }

        private void UpdateChecklist(ChecklistEntity? checklist)
        {
            if (checklist == null)
            {
                return;
            }

            var items = checklist.Items;
            checklist.Items = null;

            _context.Checklists.Update(checklist);
            var initialItems = _context.Items.Where(x => x.ChecklistId == checklist.Id).ToList();

            var oldItems = initialItems.IntersectBy(items.Select(x=> x.Id), x => x.Id).ToList();
            var newItems = items.ExceptBy(oldItems.Select(x => x.Id), x => x.Id).ToList();
            var itemsToDelete = initialItems.ExceptBy(oldItems.Select(x => x.Id), x => x.Id).ToList();

            _context.Items.UpdateRange(oldItems);
            _context.Items.RemoveRange(itemsToDelete);
            _context.Items.AddRange(newItems);
        }

        private void UpdateTable(TableEntity? table)
        {
            if (table == null)
            {
                return;
            }
            _context.Tables.Update(table);
        }
    }
}
