using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;

namespace Traki.Infrastructure.Repositories
{
    public class DrawingsRepository : IDrawingsRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public DrawingsRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Drawing>> GetDrawings(int productId)
        {
            var drawings = await _context.Drawings.Where(x=> x.ProductId == productId)
                            .Include(x=> x.Defects).ToListAsync();

            return _mapper.Map<IEnumerable<Drawing>>(drawings);
        }

        public async Task<Drawing> GetDrawing(int productId, int drawingId)
        {
            var drawing = await _context.Drawings.Where(x => x.Id == drawingId).FirstOrDefaultAsync();
            return _mapper.Map<Drawing>(drawing);
        }
    }
}
