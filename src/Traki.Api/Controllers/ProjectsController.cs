using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Project;
using Traki.Api.Handlers;
using Traki.Api.Models.Project;

namespace Traki.Api.Controllers
{
    [Route("projects")]
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

        [HttpGet]
        public async Task<ActionResult<GetProjectsResponse>> GetProjects()
        {
            var projects = await _projectsHandler.GetProjects();

            return _mapper.Map<GetProjectsResponse>(projects);
        }

        [HttpPost]
        public async Task<ActionResult<GetProjectResponse>> PostProject(CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);

            var createdProject = await _projectsHandler.CreateProject(project);

            return CreatedAtAction("GetProject", new { projectId = createdProject.Id }, createdProject);
        }
    }
}
