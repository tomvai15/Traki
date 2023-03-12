using Microsoft.AspNetCore.Mvc;
using Traki.Api.Data;

namespace Traki.Api.Controllers
{
    [Route("api/control")]
    public class ControlController : ControllerBase
    {
        private readonly TrakiDbContext _trakiDbContext;

        public ControlController(TrakiDbContext trakiDbContext)
        {
            _trakiDbContext = trakiDbContext;
        }

        [HttpGet]
        public async Task<ActionResult> Initiliase()
        {
            bool wasCreated = _trakiDbContext.Database.EnsureCreated();

            if (!wasCreated) return Ok("No data was added");

            _trakiDbContext.AddProjects();
            _trakiDbContext.AddProducts();
            _trakiDbContext.AddTemplates();
            _trakiDbContext.AddQuestions();
            _trakiDbContext.AddChecklists();
            _trakiDbContext.AddChecklistQuestions();

            return Ok("Added data");
        }
    }
}
