using Traki.Api.Data.Repositories;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IChecklistHandler
    {
        Task CreateChecklistFromTemplate(int productId, int templateId);
    }

    public class ChecklistHandler : IChecklistHandler
    {
        private readonly IChecklistRepository _checklistRepository;
        private readonly IChecklistQuestionRepository _checklistQuestionHandler;
        private readonly IQuestionsRepository _questionsHandler;
        private readonly ITemplatesHandler _templatesHandler;

        public ChecklistHandler(IChecklistRepository checklistRepository,
            IChecklistQuestionRepository checklistQuestionHandler,
            IQuestionsRepository questionsHandler,
            ITemplatesHandler templatesHandler)
        {
            _checklistRepository = checklistRepository;
            _checklistQuestionHandler = checklistQuestionHandler;
            _questionsHandler = questionsHandler;
            _templatesHandler = templatesHandler;
        }

        public async Task CreateChecklistFromTemplate(int productId, int templateId)
        {
            var questions = await _questionsHandler.GetQuestions(templateId);
            var template = await _templatesHandler.GetTemplate(templateId);

            var checklist = new CheckList { ProductId = productId, Name = template.Name, Standard = template.Standard };

            var addedChecklist = await _checklistRepository.AddChecklist(checklist);

            var checklistQuestions = questions.Select(x => new ChecklistQuestion
            {
                ChecklistId = addedChecklist.Id,
                Title = x.Title,
                Description = x.Description,
                Comment = string.Empty,
                Evaluation = Evaluation.No
            });

            await _checklistQuestionHandler.AddChecklistQuestions(checklistQuestions);
        }
    }
}
