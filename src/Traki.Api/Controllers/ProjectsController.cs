using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Project;
using Traki.Domain.Constants;
using Traki.Domain.Models;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController: ControllerBase
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public ProjectsController(IProjectsRepository projectsRepository, IClaimsProvider claimsProvider, IMapper mapper)
        {
            _claimsProvider = claimsProvider;
            _projectsRepository = projectsRepository;
            _mapper = mapper;
        }

        [HttpGet(("{projectId}"))]
        [Authorize]
        public async Task<ActionResult<GetProjectResponse>> GetProject(int projectId)
        {
            var project = await _projectsRepository.GetProject(projectId);

            return _mapper.Map<GetProjectResponse>(project);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetProjectsResponse>> GetProjects()
        {
            var projects = await _projectsRepository.GetProjects();

            return _mapper.Map<GetProjectsResponse>(projects);
        }

        [HttpPost]
        [Authorize(Roles = Role.ProjectManager)]
        public async Task<ActionResult<GetProjectResponse>> PostProject(CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);
            _claimsProvider.TryGetUserId(out int userId);
            project.CompanyId = 1;
            project.AuthorId = userId;
            var createdProject = await _projectsRepository.CreateProject(project);

            return CreatedAtAction("GetProject", new { projectId = createdProject.Id }, createdProject);
        }

        [HttpPut("{projectId}")]
        [Authorize(Roles = Role.ProjectManager)]
        public async Task<ActionResult<GetProjectResponse>> UpdateProject(int projectId, CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);
            _claimsProvider.TryGetUserId(out int userId);
            project.CompanyId = 1;
            project.AuthorId = userId;
            await _projectsRepository.UpdateProject(project);
            return Ok();
        }
    }
}
