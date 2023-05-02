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
            var project = await _context.Projects.Include(x => x.Author)
                                .FirstOrDefaultAsync(p => p.Id == projectId);

            project.RequiresToBeNotNullEnity();

            return _mapper.Map<Project>(project);
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            var projects = await _context.Projects.Include(x=> x.Products).ThenInclude(x=> x.Drawings).ThenInclude(x=> x.Defects)
                .Include(x => x.Author).ToListAsync();
            return _mapper.Map<IEnumerable<Project>>(projects);
        }

        public async Task<Project> CreateProject(Project project)
        {
            var projectToAdd = _mapper.Map<ProjectEntity>(project);

            projectToAdd.CreationDate = DateTime.Now.ToString("s");

            var createdProject = _context.Projects.Add(projectToAdd);
            await _context.SaveChangesAsync();

            var addedProject = _mapper.Map<Project>(createdProject.Entity);
            return addedProject;
        }

        public async Task<Project> UpdateProject(Project project)
        {
            var projectEntity = await _context.Projects.Where(x=> x.Id == project.Id).FirstOrDefaultAsync();

            projectEntity.RequiresToBeNotNullEnity();

            projectEntity.Name = project.Name;
            projectEntity.ClientName = project.ClientName;
            projectEntity.ImageName = project.ImageName;
            projectEntity.Address = project.Address;
            await _context.SaveChangesAsync();

            var addedProject = _mapper.Map<Project>(projectEntity);
            return addedProject;
        }

        public async Task DeleteProject(int projectId)
        {
            var project = await _context.Projects.Include(x => x.Author)
                                .FirstOrDefaultAsync(p => p.Id == projectId);

            project.RequiresToBeNotNullEnity();
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
