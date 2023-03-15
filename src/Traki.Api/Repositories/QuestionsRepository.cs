using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Repositories
{
    public interface IQuestionsRepository
    {
        Task<Question> GetQuestion(int templateId, int questionId);
        Task<IEnumerable<Question>> GetQuestions(int templateId);
        Task UpdateQuestion(int questionId, Question questionUpdate);
    }

    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public QuestionsRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Question> GetQuestion(int templateId, int questionId)
        {
            var question = _context.Questions.Where(x => x.Id == questionId).FirstOrDefault();

            question.RequiresToBeNotNullEnity();

            return _mapper.Map<Question>(question);
        }

        public async Task<IEnumerable<Question>> GetQuestions(int templateId)
        {
            var template = await _context.Templates
                .Where(x => x.Id == templateId)
                .Include(x => x.Questions)
                .FirstOrDefaultAsync();

            template.RequiresToBeNotNullEnity();


            var questions = template.Questions.ToList();
            return _mapper.Map<IEnumerable<Question>>(questions);
        }

        public async Task UpdateQuestion(int questionId, Question questionUpdate)
        {
            var question = _context.Questions.FirstOrDefault(x => x.Id == questionId);

            question.Title = questionUpdate.Title;
            question.Description = questionUpdate.Description;

            _context.SaveChangesAsync();
        }

        public void AssignNotNullProperties<T>(T source, T destination) where T : class
        {
            var p = source.GetType().GetProperties();
        }
    }
}
