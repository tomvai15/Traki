using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Data.Repositories
{
    public interface ITemplatesRepository
    {
        Task<Template> GetTemplate(int templateId);
        Task<IEnumerable<Template>> GetTemplates(int projectId);
    }

    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public TemplatesRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Template> GetTemplate(int templateId)
        {
            var template = _context.Templates.Where(x => x.Id == templateId).FirstOrDefault();

            template.RequiresToBeNotNullEnity();

            return _mapper.Map<Template>(template);
        }

        public async Task<IEnumerable<Template>> GetTemplates(int projectId)
        {
            var project = await _context.Projects
                .Where(x => x.Id == projectId)
                .Include(x => x.Templates)
                .FirstOrDefaultAsync();

            project.RequiresToBeNotNullEnity();

            var templates = project.Templates.ToList();
            return _mapper.Map<IEnumerable<Template>>(templates);
        }
    }
}
