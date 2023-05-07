using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Traki.Domain.Extensions;
using Traki.Domain.Models.Section;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Entities.Section.Items;

namespace Traki.Infrastructure.Repositories
{
    public class ChecklistRepository : IChecklistRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ChecklistRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Checklist> CreateChecklist(Checklist checkList)
        {
            var checklistEntity = _mapper.Map<ChecklistEntity>(checkList);
            checklistEntity.Id = 0;
            checklistEntity = (await _context.Checklists.AddAsync(checklistEntity)).Entity;
            await _context.SaveChangesAsync();
            return _mapper.Map<Checklist>(checklistEntity);
        }

        public async Task<bool> DeleteChecklist(int checklistId)
        {
            var checklist = _context.Checklists.FirstOrDefault(x => x.Id == checklistId);
            if (checklist == null)
            {
                return false;
            }
            _context.Checklists.Remove(checklist);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task UpdateChecklistAnswers(Checklist checklist)
        {
            try
            {
                var checklistEntity = _mapper.Map<ChecklistEntity>(checklist);
                _context.Update(checklistEntity);
                await _context.SaveChangesAsync();
            }
            catch  (Exception e)
            {
                return;
            }
        }

        public async Task<Checklist> GetChecklist(int checklistId)
        {
            var checklist = await _context.Checklists
                                            .Where(x => x.Id == checklistId)
                                            .Include(x => x.Items).FirstOrDefaultAsync();

            return _mapper.Map<Checklist>(checklist);
        }

        public async Task<Checklist> GetSectionChecklist(int sectionId)
        {
            var checklist = await _context.Checklists
                                            .Where(x => x.SectionId == sectionId)
                                            .Include(x => x.Items).FirstOrDefaultAsync();

            return _mapper.Map<Checklist>(checklist);
        }
    }
}
