using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Data.Entities;
using Traki.Api.Exceptions;
using Traki.Api.Models;

namespace Traki.Api.Data.Repositories
{
    public interface IProjectsRepository
    {
        Task<Project> GetProject(int projectId);
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> CreateProject(Project project);
    }

    public class ProjectsRepository : IProjectsRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Project> GetProject(int projectId)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                throw new EntityNotFoundException();
            }

            return _mapper.Map<Project>(project);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            var projects = await _context.Projects.ToListAsync();
            return _mapper.Map<IEnumerable<Project>>(projects);
        }

        public async Task<Project> CreateProject(Project project)
        {
            var projectToAdd = _mapper.Map<ProjectEntity>(project);
            var createdProject = _context.Projects.Add(projectToAdd);
            await _context.SaveChangesAsync();

            var addedProject = _mapper.Map<Project>(createdProject.Entity);
            return addedProject;
        }
    }
}
