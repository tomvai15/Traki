using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Repositories
{
    public class SectionRepository : ISectionRepository
    {
        public Task<Section> CreateSection(Section section)
        {
            throw new NotImplementedException();
        }

        public Task DeleteSection(Section section)
        {
            throw new NotImplementedException();
        }

        public Task<Section> GetSection(int sectionId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Section>> GetSections(int protocolId)
        {
            throw new NotImplementedException();
        }

        public Task<Section> UpdateSection(Section section)
        {
            throw new NotImplementedException();
        }
    }
}
