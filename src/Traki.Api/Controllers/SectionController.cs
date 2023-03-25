using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Section;

namespace Traki.Api.Controllers
{
    [Route("api/sections")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        
        [HttpPut("{sectionId}")]
        public async Task<ActionResult> AddSection([FromBody]UpdateSectionRequest updateSectionRequest)
        {
            return Ok();
        }
    }
}
