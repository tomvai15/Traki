using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Checklist;
using Traki.Domain.Repositories;
using Traki.Domain.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/products/{productId}/checklists")]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistHandler _checklistHandler;
        private readonly IChecklistRepository _checklistRepository;
        private readonly IMapper _mapper;

        public ChecklistController(IChecklistHandler checklistHandler, IChecklistRepository checklistRepository, IMapper mapper)
        {
            _checklistHandler = checklistHandler;
            _checklistRepository = checklistRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetChecklistsResponse>> GetChecklists(int productId)
        {
            var checkLists = await _checklistRepository.GetChecklists(productId);

            return _mapper.Map<GetChecklistsResponse>(checkLists);
        }

        [HttpGet("{checklistId}")]
        public async Task<ActionResult<GetChecklistResponse>> GetChecklist(int productId, int checklistId)
        {
            var checkList = await _checklistRepository.GetChecklist(checklistId);

            return _mapper.Map<GetChecklistResponse>(checkList);
        }


        [HttpPost("create")]
        public async Task<ActionResult> CreateFromTemplate(int productId, [FromBody] CreateChecklistRequest createChecklistRequest)
        {
            await _checklistHandler.CreateChecklistFromTemplate(productId, createChecklistRequest.templateId);

            return Ok();
        }
    }
}
