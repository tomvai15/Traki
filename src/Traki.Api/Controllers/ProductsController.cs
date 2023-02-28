using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Project;
using Traki.Api.Handlers;
using Traki.Api.Models;

namespace Traki.Api.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProjectsHandler _projectsHandler;
        private readonly IMapper _mapper;

        public ProductsController(IProjectsHandler projectsHandler, IMapper mapper)
        {
            _projectsHandler = projectsHandler;
            _mapper = mapper;
        }

        [HttpGet(("{productId}"))]
        public async Task<ActionResult<GetProjectResponse>> GetProduct(int productId)
        {
            var project = await _projectsHandler.GetProject(productId);

            if (project == null)
            {
                return NotFound();
            }

            return _mapper.Map<GetProjectResponse>(project);
        }

        [HttpGet]
        public async Task<ActionResult<GetProjectsResponse>> GetProducts()
        {
            var projects = await _projectsHandler.GetProjects();

            return _mapper.Map<GetProjectsResponse>(projects);
        }

        [HttpPost]
        public async Task<ActionResult<GetProjectResponse>> PostProduct(CreateProjectRequest createProjectRequest)
        {
            var project = _mapper.Map<Project>(createProjectRequest);

            var createdProject = await _projectsHandler.CreateProject(project);

            return CreatedAtAction("GetProduct", new { projectId = createdProject.Id }, createdProject);
        }
    }
}
