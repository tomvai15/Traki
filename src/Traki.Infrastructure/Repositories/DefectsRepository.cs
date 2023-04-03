using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Infrastructure.Repositories
{
    public class DefectsRepository : IDefectsRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public DefectsRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Defect> CreateDefect(Defect defect)
        {
            var defectEntity = _mapper.Map<DefectEntity>(defect);

            _context.Add(defectEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Defect>(defectEntity);
        }

        public async Task<Defect> GetDefect(int defectId)
        {
            var defectEntity = await _context.Defects.Where(x => x.Id == defectId)
                .Include(x => x.DefectComments)
                .FirstOrDefaultAsync();

            return _mapper.Map<Defect>(defectEntity);
        }
    }
}
