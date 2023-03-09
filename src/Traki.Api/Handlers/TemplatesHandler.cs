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
