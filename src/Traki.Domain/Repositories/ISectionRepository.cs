using Traki.Domain.Models.Section;

namespace Traki.Domain.Repositories
{
    public interface ISectionRepository
    {
        Task<Section> GetSection(int sectionId);
        Task DeleteSection(Section section);
        Task<IEnumerable<Section>> GetSections(int protocolId);
        Task<Section> CreateSection(Section section);
        Task<Section> UpdateSection(Section section);
    }
}
