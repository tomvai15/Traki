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
    [Authorize]
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

            return Ok(_mapper.Map<GetProjectResponse>(project));
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<GetProjectsResponse>> GetProjects()
        {
            var projects = await _projectsRepository.GetProjects();


            return Ok(_mapper.Map<GetProjectsResponse>(projects));
        }

        [HttpPost]
        [Authorize]
        [Authorize(Roles = Role.ProjectManager)]
        public async Task<ActionResult<GetProjectResponse>> PostProject(CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);
            _claimsProvider.TryGetUserId(out int userId);
            project.CompanyId = 1;
            project.AuthorId = userId;
            project.Author = null;
            var createdProject = await _projectsRepository.CreateProject(project);

            var reponse = new GetProjectResponse {
                Project = _mapper.Map<ProjectDto>(createdProject)
            };

            return Ok(reponse);
        }

        [HttpPut("{projectId}")]
        [Authorize(Roles = Role.ProjectManager)]
        [Authorize(Policy = AuthPolicy.ProjectIdInRouteValidation)]
        public async Task<ActionResult> UpdateProject(int projectId, CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);
            _claimsProvider.TryGetUserId(out int userId);
            project.CompanyId = 1;
            project.AuthorId = userId;
            await _projectsRepository.UpdateProject(project);
            return Ok();
        }

        [HttpDelete("{projectId}")]
        [Authorize(Roles = Role.ProjectManager)]
        [Authorize(Policy = AuthPolicy.ProjectIdInRouteValidation)]
        public async Task<ActionResult> DeleteProject(int projectId)
        {
            await _projectsRepository.DeleteProject(projectId);
            return Ok();
        }
    }
}
