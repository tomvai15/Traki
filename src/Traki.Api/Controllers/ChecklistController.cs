﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Checklist;
using Traki.Api.Contracts.Template;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/products/{productId}/checklists")]
    public class ChecklistController : ControllerBase
    {
        private readonly IChecklistHandler _checklistHandler;
        private readonly IMapper _mapper;

        public ChecklistController(IChecklistHandler checklistHandler, IMapper mapper)
        {
            _checklistHandler = checklistHandler;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetChecklistsResponse>> GetChecklists(int productId)
        {
            var checkLists = await _checklistHandler.GetChecklists(productId);

            return _mapper.Map<GetChecklistsResponse>(checkLists);
        }

        [HttpGet("{checklistId}")]
        public async Task<ActionResult<GetChecklistResponse>> GetChecklist(int productId, int checklistId)
        {
            var checkList = await _checklistHandler.GetChecklist(checklistId);

            return _mapper.Map<GetChecklistResponse>(checkList);
        }
    }
}