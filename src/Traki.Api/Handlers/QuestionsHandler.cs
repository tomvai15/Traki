using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IQuestionsHandler
    {
        Task<IEnumerable<Question>> GetQuestions(int templateId);
    }

    public class QuestionsHandler : IQuestionsHandler
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public QuestionsHandler(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Question>> GetQuestions(int templateId)
        {
            var template = await _context.Templates
                .Where(x => x.Id == templateId)
                .Include(x => x.Questions)
                .FirstOrDefaultAsync();

            template.RequiresNotNullEnity();


            var questions = template.Questions.ToList();
            return _mapper.Map<IEnumerable<Question>>(questions);
        }
    }
}
