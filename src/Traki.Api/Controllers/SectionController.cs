using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Section;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Section;
using Traki.Domain.Services;

namespace Traki.Api.Controllers
{
    [Route("api/protocols/{protocolId}/sections")]
    [ApiController]
    [Authorize]
    public class SectionController : ControllerBase
    {
        private readonly ISectionHandler _sectionHandler;
        private readonly IProtocolService _protocolService;
        private readonly IMapper _mapper;

        public SectionController(ISectionHandler sectionHandler, IProtocolService protocolService, IMapper mapper)
        {
            _sectionHandler = sectionHandler;
            _protocolService = protocolService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<GetSectionsResponse>> GetSections(int protocolId)
        {
            var protocol = await _protocolService.FindProtocol(protocolId);

            var getSectionRequest = new GetSectionsResponse { Sections = _mapper.Map<IEnumerable<SectionDto>>(protocol.Sections) };

            return Ok(getSectionRequest);
        }

        [HttpGet("{sectionId}")]
        public async Task<ActionResult<GetSectionResponse>> GetSection(int protocolId, int sectionId)
        {
            var section = await _protocolService.FindSection(protocolId, sectionId);

            var getSectionRequest = new GetSectionResponse { Section = _mapper.Map<SectionDto>(section) };

            return Ok(getSectionRequest);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSection(int protocolId, [FromBody] UpdateSectionRequest updateSectionRequest)
        {
            var section = _mapper.Map<Section>(updateSectionRequest.Section);

            await _protocolService.AddSection(protocolId, section);

            return Ok();
        }

        [HttpPut("{sectionId}")]
        public async Task<ActionResult> UpdateSection(int protocolId, [FromBody]UpdateSectionRequest updateSectionRequest)
        {
            var section = _mapper.Map<Section>(updateSectionRequest.Section);
            await _protocolService.UpdateSection(section);
            return Ok();
        }

        [HttpPut("{sectionId}/answers")]
        public async Task<ActionResult> UpdateSectionAnswers(int protocolId, [FromBody] UpdateSectionAnswersRequest updateSectionRequest)
        {
            var section = _mapper.Map<Section>(updateSectionRequest.Section);
            await _sectionHandler.UpdateSectionAnswers(protocolId, section);
            return Ok();
        }

        [HttpDelete("{sectionId}")]
        public async Task<ActionResult> DeleteSection(int protocolId, int sectionId)
        {
            await _protocolService.DeleteSection(protocolId, sectionId);
            return Ok();
        }
    }
}
