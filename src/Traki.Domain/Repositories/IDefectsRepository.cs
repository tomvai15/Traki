using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IDefectsRepository
    {
        Task<Defect> CreateDefect(Defect defect);
        Task<Defect> UpdateDefect(Defect defect);
        Task<Defect> GetDefect(int defectId);
        Task<IEnumerable<Defect>> GetDefectsByQuery(Func<Defect, bool> filter);
    }
}
