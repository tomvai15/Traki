using Traki.Domain.Models;
using Traki.Domain.Models.Section;

namespace Traki.Domain.Repositories
{
    public interface IChecklistRepository
    {
        Task<CheckList> AddChecklist(CheckList checkList);
        Task<IEnumerable<CheckList>> GetChecklists(int productId);
        Task<CheckList> GetChecklist(int checklistId);

        Task<Checklist> CreateChecklist(Checklist checkList);
        Task<bool> DeleteChecklist(int checklistId);
    }
}
