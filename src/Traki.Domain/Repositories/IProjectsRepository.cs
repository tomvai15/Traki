using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IProjectsRepository
    {
        Task<Project> GetProject(int projectId);
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> CreateProject(Project project);
    }
}
