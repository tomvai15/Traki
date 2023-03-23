using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IChecklistQuestionRepository
    {
        Task AddChecklistQuestions(IEnumerable<ChecklistQuestion> checklistQuestions);
        Task<IEnumerable<ChecklistQuestion>> GetChecklistQuestions(int checklistId);
        Task UpdateChecklistQuestions(IEnumerable<ChecklistQuestion> checklistQuestions);
    }
}
