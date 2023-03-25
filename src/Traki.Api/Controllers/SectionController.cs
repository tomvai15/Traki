using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Section;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;


namespace Traki.Api.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionHandler _sectionHandler;
        private readonly IMapper _mapper;

        public SectionController(ISectionHandler sectionHandler, IMapper mapper)
        {
            _sectionHandler = sectionHandler;
            _mapper = mapper;
        }

        [HttpPut("{sectionId}")]
        public async Task<ActionResult> AddSection([FromBody]UpdateSectionRequest updateSectionRequest)
        {

            var section = _mapper.Map<Section>(updateSectionRequest.section);

            await _sectionHandler.AddOrUpdateSection(section); ;

            return Ok();
        }

        [HttpGet("{sectionId}")]
        public async Task<ActionResult<GetSectionRequest>> GetSection(int sectionId)
        {

            var section = await _sectionHandler.GetSection(sectionId);

            var getSectionRequest = new GetSectionRequest { Section = _mapper.Map<SectionDto>(section) };

            return Ok(getSectionRequest);
        }
    }
}
