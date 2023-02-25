using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Models.Project;

namespace Traki.Api.Handlers
{
    public interface IProjectsHandler
    {
        Task<Project> GetProject(int projectId);
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
    }
}
