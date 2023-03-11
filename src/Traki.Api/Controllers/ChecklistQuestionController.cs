using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Checklist;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/products/{productId}/checklists")]
    public class ChecklistQuestionController : ControllerBase
    {
        private readonly IChecklistHandler _checklistHandler;
        private readonly IMapper _mapper;

        public ChecklistQuestionController(IChecklistHandler checklistHandler, IMapper mapper)
        {
            _checklistHandler = checklistHandler;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetChecklistsResponse>> GetChecklistQuestions(int checklistId)
        {
            var checkLists = await _checklistHandler.GetChecklists(checklistId);

            return _mapper.Map<GetChecklistsResponse>(checkLists);
        }

        [HttpGet("{checklistQuestionId}")]
        public async Task<ActionResult<GetChecklistResponse>> GetChecklistQuestion(int checklistId, int checklistQuestionId)
        {
            var checkList = await _checklistHandler.GetChecklist(checklistId);

            return _mapper.Map<GetChecklistResponse>(checkList);
        }
    }
}
