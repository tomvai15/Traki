using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/drawings/{drawingId}/defects")]
    public class DefectsController : ControllerBase
    {
        private readonly IDefectHandler _defectHandler;
        private readonly IDefectsRepository _defectsRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public DefectsController(IDefectHandler defectHandler, IDefectsRepository defectsRepository, IClaimsProvider claimsProvider, IMapper mapper)
        {
            _defectHandler = defectHandler;
            _defectsRepository = defectsRepository;
            _claimsProvider = claimsProvider;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GetDefectResponse>> CreateDefect(int drawingId, [FromBody] CreateDefectRequest createDefectRequest)
        {
            var defect = _mapper.Map<Defect>(createDefectRequest.Defect);
            _claimsProvider.TryGetUserId(out int userId);
            defect = await _defectHandler.CreateDefect(userId, drawingId, defect);
            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(defect)
            };
            return Ok(response);
        }

        [HttpPut("{defectId}")]
        [Authorize]
        public async Task<ActionResult<GetDefectResponse>> UpdateDefect(int drawingId, int defectId, [FromBody] CreateDefectRequest createDefectRequest)
        {
            var defect = _mapper.Map<Defect>(createDefectRequest.Defect);
            _claimsProvider.TryGetUserId(out int userId);

            defect.Id = defectId;
            defect.DrawingId = drawingId;

            defect = await _defectHandler.CreateDefectStatusChange(userId, defect);

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(defect)
            };
            return Ok(response);
        }

        [HttpGet("{defectId}")]
        public async Task<ActionResult<GetDefectResponse>> GetDefect(int drawingId, int defectId)
        {
            var defect = await _defectsRepository.GetDefect(defectId);
            var defectDto = _mapper.Map<DefectDto>(defect);
            return Ok(new GetDefectResponse { Defect = defectDto });
        }

        [HttpPost("{defectId}/comments")]
        [Authorize]
        public async Task<ActionResult> AddDefectComment(int drawingId, int defectId, [FromBody] CreateDefectCommentRequest createDefectCommentRequest)
        {
            var defectComment = _mapper.Map<DefectComment>(createDefectCommentRequest.DefectComment);
            _claimsProvider.TryGetUserId(out int userId);

            defectComment.DefectId = defectId;
            await _defectHandler.CreateDefectComment(userId, defectComment);
            return Ok();
        }
    }
}
