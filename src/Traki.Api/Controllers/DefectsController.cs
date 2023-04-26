using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/drawings/{drawingId}/defects")]
    public class DefectsController : ControllerBase
    {
        private readonly IDefectHandler _defectHandler;
        private readonly IDefectsRepository _defectsRepository;
        private readonly IDefectCommentRepository _defectCommentRepository;
        private readonly IStatusChangeRepository _statusChangeRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public DefectsController(IDefectHandler defectHandler, IDefectsRepository defectsRepository, IDefectCommentRepository defectCommentRepository, IStatusChangeRepository statusChangeRepository, IClaimsProvider claimsProvider, IMapper mapper)
        {
            _defectHandler = defectHandler;
            _defectsRepository = defectsRepository;
            _defectCommentRepository = defectCommentRepository;
            _statusChangeRepository = statusChangeRepository;
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
        public async Task<ActionResult<GetDefectResponse>> CreateDefect(int drawingId, int defectId, [FromBody] CreateDefectRequest createDefectRequest)
        {
            var defect = _mapper.Map<Defect>(createDefectRequest.Defect);
            _claimsProvider.TryGetUserId(out int userId);
            defect.AuthorId = userId;

            var def = await _defectsRepository.GetDefect(defectId);
            var statusChange = new StatusChange
            {
                From = def.Status,
                To = defect.Status,
                Date = DateTime.Now.ToString("s"),
                AuthorId = userId,
                DefectId = defectId,
            };


            defect = await _defectsRepository.UpdateDefect(defect);
            await _statusChangeRepository.CreateStatusChange(statusChange);

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
            defectComment.Date = DateTime.Now.ToString("s");
            defectComment.AuthorId = userId != 0 ? userId : 1;

            await _defectCommentRepository.CreateDefectComment(defectComment);
            return Ok();
        }

        [HttpPost("{defectId}/statuschanges")]
        public async Task<ActionResult> AddStatusChange(int drawingId, int defectId, [FromBody] CreateStatusChangeRequest createStatusChangeRequest)
        {
           // TODO: reconsider usingthis endpoint
            return Ok();
        }
    }
}
