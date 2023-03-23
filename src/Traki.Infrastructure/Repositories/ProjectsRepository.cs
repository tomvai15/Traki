using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Traki.Domain.Extensions;

namespace Traki.Infrastructure.Repositories
{
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

            project.RequiresToBeNotNullEnity();

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
