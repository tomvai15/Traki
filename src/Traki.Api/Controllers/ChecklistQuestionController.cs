using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.ChecklistQuestion;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/checklists/{checklistId}/checklistquestions")]
    public class ChecklistQuestionController : ControllerBase
    {
        private readonly IChecklistQuestionRepository _checklistQuestionRepository;
        private readonly IMapper _mapper;

        public ChecklistQuestionController(IChecklistQuestionRepository checklistQuestionRepository, IMapper mapper)
        {
            _checklistQuestionRepository = checklistQuestionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetChecklistQuestionsResponse>> GetChecklistQuestions(int checklistId)
        {
            var checkListQuestions = await _checklistQuestionRepository.GetChecklistQuestions(checklistId);

            return _mapper.Map<GetChecklistQuestionsResponse>(checkListQuestions);
        }


        [HttpPut]
        public async Task<ActionResult<GetChecklistQuestionsResponse>> UpdateChecklistQuestions([FromBody] UpdateChecklistQuestionsRequest updateChecklistQuestionsRequest)
        {
            var checkListQuestions = _mapper.Map<IEnumerable<ChecklistQuestion>>(updateChecklistQuestionsRequest.ChecklistQuestions);
            await _checklistQuestionRepository.UpdateChecklistQuestions(checkListQuestions);

            return Ok();
        }
    }
}
