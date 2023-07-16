using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IChecklistRepository
    {
        Task<Checklist> GetChecklist(int checklistId);
        Task<Checklist> GetSectionChecklist(int sectionId);
        Task<Checklist> CreateChecklist(Checklist checkList);
        Task<bool> DeleteChecklist(int checklistId);
    }
}
