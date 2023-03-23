using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Project;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController: ControllerBase
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectsRepository projectsRepository, IMapper mapper)
        {
            _projectsRepository = projectsRepository;
            _mapper = mapper;
        }

        [HttpGet(("{projectId}"))]
        public async Task<ActionResult<GetProjectResponse>> GetProject(int projectId)
        {
            var project = await _projectsRepository.GetProject(projectId);

            return _mapper.Map<GetProjectResponse>(project);
        }

        [HttpGet]
        public async Task<ActionResult<GetProjectsResponse>> GetProjects()
        {
            var projects = await _projectsRepository.GetProjects();

            return _mapper.Map<GetProjectsResponse>(projects);
        }

        [HttpPost]
        public async Task<ActionResult<GetProjectResponse>> PostProject(CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);

            var createdProject = await _projectsRepository.CreateProject(project);

            return CreatedAtAction("GetProject", new { projectId = createdProject.Id }, createdProject);
        }
    }
}
