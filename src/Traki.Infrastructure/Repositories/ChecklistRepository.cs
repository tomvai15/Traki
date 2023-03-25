using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Traki.Domain.Extensions;
using Traki.Domain.Models.Section;

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

        public Task<Checklist> CreateChecklist(Checklist checkList)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteChecklist(int checklistId)
        {
            throw new NotImplementedException();
        }

        public async Task<CheckList> GetChecklist(int checklistId)
        {
            var checklist = await _context.OldChecklists.FirstOrDefaultAsync(x => x.Id == checklistId);

            checklist.RequiresToBeNotNullEnity();
            return _mapper.Map<CheckList>(checklist);
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
    }
}
