using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/drawings/{drawingId}/defects")]
    public class DefectsController : ControllerBase
    {
        private readonly IDefectsRepository _defectsRepository;
        private readonly IDefectCommentRepository _defectCommentRepository;
        private readonly IMapper _mapper;

        public DefectsController(IDefectsRepository defectsRepository,  IMapper mapper, IDefectCommentRepository defectCommentRepository)
        {
            _defectsRepository = defectsRepository;
            _mapper = mapper;
            _defectCommentRepository = defectCommentRepository;
        }

        [HttpPost]
        public async Task<ActionResult> AddDefect(int drawingId, [FromBody] CreateDefectRequest createDefectRequest)
        {
            var defect = _mapper.Map<Defect>(createDefectRequest.Defect);
            defect.DrawingId = drawingId;
            defect.Status = DefectStatus.NotFixed;
            await _defectsRepository.CreateDefect(defect);
            return Ok();
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
            defectComment.DefectId = defectId;
            defectComment.Author = "TV";
            defectComment.Date = DateTime.Now.ToString("G");

            await _defectCommentRepository.CreateDefectComment(defectComment);
            return Ok();
        }
    }
}
