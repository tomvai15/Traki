using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IChecklistRepository
    {
        Task<CheckList> AddChecklist(CheckList checkList);
        Task<IEnumerable<CheckList>> GetChecklists(int productId);
        Task<CheckList> GetChecklist(int checklistId);
    }
}
