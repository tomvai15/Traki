using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Traki.Api.Data;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface ITemplatesHandler
    {
        Task<Template> GetTemplate(int projectId, int templateId);
        Task<IEnumerable<Template>> GetTemplates(int projectId);
    }

    public class TemplatesHandler : ITemplatesHandler
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public TemplatesHandler(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Template> GetTemplate(int projectId, int templateId)
        {
            var template = _context.Templates.Where(x => x.Id == templateId).FirstOrDefault();

            template.RequiresNotNullEnity();

            return _mapper.Map<Template>(template);
        }

        public async Task<IEnumerable<Template>> GetTemplates(int projectId)
        {
            var project =  await _context.Projects
                .Where(x=> x.Id == projectId)
                .Include(x=> x.Templates)
                .FirstOrDefaultAsync();

            project.RequiresNotNullEnity();

            var templates = project.Templates.ToList();
            return _mapper.Map<IEnumerable<Template>>(templates);
        }
    }
}
