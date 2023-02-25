using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Project;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController: ControllerBase
    {
        private readonly IProjectsHandler _projectsHandler;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectsHandler projectsHandler, IMapper mapper)
        {
            _projectsHandler = projectsHandler;
            _mapper = mapper;
        }

        [HttpGet(("{projectId}"))]
        public async Task<ActionResult<GetProjectResponse>> GetProject(int projectId)
        {
            var project = await _projectsHandler.GetProject(projectId);

            if (project == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetProjectResponse>(project);
        }
    }
}
