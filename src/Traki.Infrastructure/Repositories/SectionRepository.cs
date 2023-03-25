using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public SectionRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Section> CreateSection(Section section)
        {
            var sectionEntity = _mapper.Map<SectionEntity>(section);
            sectionEntity = (await _context.Sections.AddAsync(sectionEntity)).Entity;
            await _context.SaveChangesAsync();
            return _mapper.Map<Section>(sectionEntity);
        }

        public async Task<Section> UpdateSection(Section section)
        {
            var sectionEntity = await _context.Sections.FirstOrDefaultAsync(p => p.Id == section.Id);
            sectionEntity.Name = section.Name;
            sectionEntity.Priority = section.Priority;
            await _context.SaveChangesAsync();
            return _mapper.Map<Section>(sectionEntity);
        }

        public async Task<Section> GetSection(int sectionId)
        {
            var sections = await _context.Sections.ToListAsync();

            var section = sections.Where(p => p.Id == sectionId).FirstOrDefault();
            if (section == null)
            {
                return null;
            }
            return _mapper.Map<Section>(section);
        }
    }
}
