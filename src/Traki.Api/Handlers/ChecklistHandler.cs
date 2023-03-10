using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IChecklistHandler
    {
        Task<IEnumerable<CheckList>> GetChecklists(int productId);
    }

    public class ChecklistHandler : IChecklistHandler
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ChecklistHandler(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
