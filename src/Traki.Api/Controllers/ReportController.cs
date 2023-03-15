using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Project;
using Traki.Api.Models;

namespace Traki.Api.Controllers
{
    [Route("api/report")]
    public class ReportController: ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> GeneratePdf()
        {
            //  var project = _mapper.Map<Project>(createProjectRequest);

            //   var createdProject = await _projectsHandler.CreateProject(project);

            //    return CreatedAtAction("GetProject", new { projectId = createdProject.Id }, createdProject);

            return Ok();
        }
    }
}
