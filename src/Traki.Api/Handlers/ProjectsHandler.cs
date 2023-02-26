using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Models.Project;

namespace Traki.Api.Handlers
{
    public interface IProjectsHandler
    {
        Task<Project> GetProject(int projectId);
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> CreateProject(Project project);
    }

    public class ProjectsHandler : IProjectsHandler
    {
        private readonly TrakiDbContext _context;

        public ProjectsHandler(TrakiDbContext context)
        {
            _context = context;
        }
        public async Task<Project> GetProject(int projectId)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<Project> CreateProject(Project project)
        {
           var createdProject = _context.Projects.Add(project);
           await _context.SaveChangesAsync();

            return createdProject.Entity;
        }
    }
}
