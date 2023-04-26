using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Drawing;

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
            var drawings = await _context.Drawings.Where(x => x.ProductId == productId)
                            .Include(x => x.Defects).ThenInclude(x => x.Author).ToListAsync();

            return _mapper.Map<IEnumerable<Drawing>>(drawings);
        }

        public async Task<Drawing> GetDrawing(int drawingId)
        {
            var drawing = await _context.Drawings.Where(x => x.Id == drawingId).FirstOrDefaultAsync();
            return _mapper.Map<Drawing>(drawing);
        }

        public async Task CreateDrawing(Drawing drawing)
        {
            var drawingEntity = _mapper.Map<DrawingEntity>(drawing);

            _context.Drawings.Add(drawingEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDrawing(int drawingId)
        {
            var drawing = await _context.Drawings.Where(x => x.Id == drawingId).FirstOrDefaultAsync();
            _context.Drawings.Remove(drawing);
            await _context.SaveChangesAsync();
        }
    }
}
