using Traki.Domain.Models;
using Traki.Domain.Models.Section;

namespace Traki.Domain.Repositories
{
    public interface IChecklistRepository
    {
        Task<CheckList> AddChecklist(CheckList checkList);
        Task<IEnumerable<CheckList>> GetChecklists(int productId);
        Task<CheckList> GetCheckList(int checklistId);

        Task UpdateChecklistAnswers(Checklist checkList);
        Task<Checklist> GetChecklist(int checklistId);
        Task<Checklist> GetSectionChecklist(int sectionId);
        Task<Checklist> CreateChecklist(Checklist checkList);
        Task<bool> DeleteChecklist(int checklistId);
    }
}
