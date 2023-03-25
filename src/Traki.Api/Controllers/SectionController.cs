using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Section;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IMapper _mapper;

        public SectionController(IProjectsRepository projectsRepository, IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _mapper = mapper;
        }

        [HttpPut("{sectionId}")]
        public async Task<ActionResult> AddSection([FromBody]UpdateSectionRequest updateSectionRequest)
        {

            var section = _mapper.Map<Section>(updateSectionRequest.section);
            return Ok();
        }
    }
}
