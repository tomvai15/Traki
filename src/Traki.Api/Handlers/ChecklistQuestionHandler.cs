using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IChecklistQuestionHandler
    {
        Task<IEnumerable<ChecklistQuestion>> GetChecklistQuestions(int checklistId);
        Task UpdateChecklistQuestions(IEnumerable<ChecklistQuestion> checklistQuestions);
    }

    public class ChecklistQuestionHandler : IChecklistQuestionHandler
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ChecklistQuestionHandler(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ChecklistQuestion>> GetChecklistQuestions(int checklistId)
        {
            var checklist = await _context.Checklists
                .Where(x => x.Id == checklistId)
                .Include(x => x.ChecklistQuestions)
                .FirstOrDefaultAsync();

            checklist.RequiresToBeNotNullEnity();

            var checklistQuestions = checklist.ChecklistQuestions.ToList();
            return _mapper.Map<IEnumerable<ChecklistQuestion>>(checklistQuestions);
        }

        public async Task UpdateChecklistQuestions(IEnumerable<ChecklistQuestion> checklistQuestions)
        {
            foreach (var checklistQuestion in checklistQuestions)
            {
                var checklistQuestionUpdate = await _context.CheckListQuestions
                    .FirstOrDefaultAsync(x => x.Id == checklistQuestion.Id);

                checklistQuestionUpdate.RequiresToBeNotNullEnity();

                checklistQuestionUpdate.Comment = checklistQuestion.Comment;
                checklistQuestionUpdate.Evaluation = checklistQuestion.Evaluation;
            }
            await _context.SaveChangesAsync();
        }
    }
}
