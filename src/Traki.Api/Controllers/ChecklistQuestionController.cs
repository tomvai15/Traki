using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.ChecklistQuestion;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/checklists/{checklistId}/checklistquestions")]
    public class ChecklistQuestionController : ControllerBase
    {
        private readonly IChecklistQuestionHandler _checklistQuestionHandler;
        private readonly IMapper _mapper;

        public ChecklistQuestionController(IChecklistQuestionHandler checklistQuestionHandler, IMapper mapper)
        {
            _checklistQuestionHandler = checklistQuestionHandler;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetChecklistQuestionsResponse>> GetChecklistQuestions(int checklistId)
        {
            var checkListQuestions = await _checklistQuestionHandler.GetChecklistQuestions(checklistId);

            return _mapper.Map<GetChecklistQuestionsResponse>(checkListQuestions);
        }
    }
}
