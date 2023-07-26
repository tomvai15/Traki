using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Section;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
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

        [HttpPut("{sectionId}")]
        public async Task<ActionResult> UpdateSection(int protocolId, [FromBody]UpdateSectionRequest updateSectionRequest)
        {
            var section = _mapper.Map<Section>(updateSectionRequest.Section);
            await _sectionHandler.AddOrUpdateSection(protocolId, section);
            return Ok();
        }

        [HttpPut("{sectionId}/answers")]
        public async Task<ActionResult> UpdateSectionAnswers(int protocolId, [FromBody] UpdateSectionAnswersRequest updateSectionRequest)
        {
            var section = _mapper.Map<Section>(updateSectionRequest.Section);
            await _sectionHandler.UpdateSectionAnswers(protocolId, section);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateSection(int protocolId, [FromBody] UpdateSectionRequest updateSectionRequest)
        {
            var section = _mapper.Map<Section>(updateSectionRequest.Section);

            await _sectionHandler.AddOrUpdateSection(protocolId, section); ;

            return Ok();
        }

        [HttpGet("{sectionId}")]
        public async Task<ActionResult<GetSectionResponse>> GetSection(int sectionId)
        {
            var section = await _sectionHandler.GetSection(sectionId);

            var getSectionRequest = new GetSectionResponse { Section = _mapper.Map<SectionDto>(section) };

            return Ok(getSectionRequest);
        }

        [HttpDelete("{sectionId}")]
        public async Task<ActionResult> DeleteSection(int sectionId)
        {
            await _sectionHandler.DeleteSection(sectionId);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<GetSectionsResponse>> GetSections(int protocolId)
        {
            var protocol = await _protocolService.FindProtocol(protocolId);

            var getSectionRequest = new GetSectionsResponse { Sections = _mapper.Map<IEnumerable<SectionDto>>(protocol.Sections) };

            return Ok(getSectionRequest);
        }
    }
}
