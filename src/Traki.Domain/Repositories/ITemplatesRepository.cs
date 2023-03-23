using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface ITemplatesRepository
    {
        Task<Template> GetTemplate(int templateId);
        Task<IEnumerable<Template>> GetTemplates(int projectId);
    }
}
