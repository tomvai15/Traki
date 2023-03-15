using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.ChecklistQuestion;
using Traki.Api.Repositories;
using Traki.Api.Models;

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


        [HttpPut]
        public async Task<ActionResult<GetChecklistQuestionsResponse>> UpdateChecklistQuestions([FromBody] UpdateChecklistQuestionsRequest updateChecklistQuestionsRequest)
        {
            var checkListQuestions = _mapper.Map<IEnumerable<ChecklistQuestion>>(updateChecklistQuestionsRequest.ChecklistQuestions);
            await _checklistQuestionHandler.UpdateChecklistQuestions(checkListQuestions);

            return Ok();
        }
    }
}
