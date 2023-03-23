using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IQuestionsRepository
    {
        Task<Question> GetQuestion(int templateId, int questionId);
        Task<IEnumerable<Question>> GetQuestions(int templateId);
        Task UpdateQuestion(int questionId, Question questionUpdate);
    }
}
