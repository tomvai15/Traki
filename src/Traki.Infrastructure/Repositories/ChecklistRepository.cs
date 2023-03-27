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

        public async Task<CheckList> AddChecklist(CheckList checkList)
        {
            var checkListEntity = _mapper.Map<OldChecklistEntity>(checkList);
            var addedChecklist = await _context.OldChecklists.AddAsync(checkListEntity);

            await _context.SaveChangesAsync();
            return _mapper.Map<CheckList>(addedChecklist.Entity);
        }

        public async Task<Checklist> CreateChecklist(Checklist checkList)
        {
            var checklistEntity = _mapper.Map<ChecklistEntity>(checkList);
          //  checklistEntity.Items = new ItemEntity[0];
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

        public async Task<CheckList> GetCheckList(int checklistId)
        {
            var checklist = await _context.OldChecklists.FirstOrDefaultAsync(x => x.Id == checklistId);

            checklist.RequiresToBeNotNullEnity();
            return _mapper.Map<CheckList>(checklist);
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

        public async Task<IEnumerable<CheckList>> GetChecklists(int productId)
        {
            var product = await _context.Products
                .Where(x => x.Id == productId)
                .Include(x => x.CheckLists)
                .FirstOrDefaultAsync();

            product.RequiresToBeNotNullEnity();

            var checklists = product.CheckLists.ToList();
            return _mapper.Map<IEnumerable<CheckList>>(checklists);
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
