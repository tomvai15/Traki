using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Drawing.Defect;
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
        private readonly IDefectsRepository _defectsRepository;
        private readonly IDefectCommentRepository _defectCommentRepository;
        private readonly IStatusChangeRepository _statusChangeRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public DefectsController(IClaimsProvider claimsProvider, IDefectsRepository defectsRepository,  IMapper mapper, IDefectCommentRepository defectCommentRepository, IStatusChangeRepository statusChangeRepository)
        {
            _claimsProvider = claimsProvider;
            _defectsRepository = defectsRepository;
            _mapper = mapper;
            _defectCommentRepository = defectCommentRepository;
            _statusChangeRepository = statusChangeRepository;
        }

        [HttpPost]
        public async Task<ActionResult<GetDefectResponse>> CreateDefect(int drawingId, [FromBody] CreateDefectRequest createDefectRequest)
        {
            var defect = _mapper.Map<Defect>(createDefectRequest.Defect);
            _claimsProvider.TryGetUserId(out int userId);
            defect.UserId = userId != 0 ? userId : 1;
            defect.DrawingId = drawingId;
            defect.Status = DefectStatus.NotFixed;
            defect = await _defectsRepository.CreateDefect(defect);
            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(defect)
            };
            return Ok(response);
        }

        [HttpPut("{defectId}")]
        public async Task<ActionResult<GetDefectResponse>> CreateDefect(int drawingId, int defectId, [FromBody] CreateDefectRequest createDefectRequest)
        {
            var defect = _mapper.Map<Defect>(createDefectRequest.Defect);
            _claimsProvider.TryGetUserId(out int userId);
            defect.UserId = userId;

            var def = await _defectsRepository.GetDefect(defectId);
            var statusChange = new StatusChange
            {
                From = def.Status,
                To = defect.Status,
                Date = DateTime.Now.ToString("s"),
                UserId = userId,
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
        public async Task<ActionResult> AddDefectComment(int drawingId, int defectId, [FromBody] CreateDefectCommentRequest createDefectCommentRequest)
        {
            var defectComment = _mapper.Map<DefectComment>(createDefectCommentRequest.DefectComment);
            _claimsProvider.TryGetUserId(out int userId);

            defectComment.DefectId = defectId;
            defectComment.Author = "TV";
            defectComment.Date = DateTime.Now.ToString("s");
            defectComment.UserId = userId != 0 ? userId : 1;

            await _defectCommentRepository.CreateDefectComment(defectComment);
            return Ok();
        }

        [HttpPost("{defectId}/statuschanges")]
        public async Task<ActionResult> AddStatusChange(int drawingId, int defectId, [FromBody] CreateStatusChangeRequest createStatusChangeRequest)
        {
            /*
            var defectComment = _mapper.Map<DefectComment>(createDefectCommentRequest.DefectComment);
            _claimsProvider.TryGetUserId(out int userId);

            defectComment.DefectId = defectId;
            defectComment.Author = "TV";
            defectComment.Date = DateTime.Now.ToString("G");
            defectComment.UserId = userId != 0 ? userId : 1;

            await _defectCommentRepository.CreateDefectComment(defectComment);*/
            return Ok();
        }
    }
}
