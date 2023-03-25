using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;

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

        public Task<Section> CreateSection(Section section)
        {
            throw new NotImplementedException();
        }

        public async Task<Section> GetSection(int sectionId)
        {
            var section = await _context.Sections.FirstOrDefaultAsync(p => p.Id == sectionId);
            if (section == null)
            {
                return null;
            }
            return _mapper.Map<Section>(section);
        }
    }
}
