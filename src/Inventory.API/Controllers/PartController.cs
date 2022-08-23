using Inventory.Domain.Parts;
using Microsoft;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private readonly IPartsHandler partsHandler;

        public PartController(IPartsHandler partsHandler)
        {
            Requires.NotNull(partsHandler, nameof(partsHandler));

            this.partsHandler = partsHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Part>>> GetParts()
        {
            return await partsHandler.GetParts();
        }
    }
}
