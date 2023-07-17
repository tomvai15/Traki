using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class ProjectsRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public ProjectsRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetProject_ReturnsProject()
        {
            int projectId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProjectsRepository(context, _mapper);

            var result = await repository.GetProject(projectId);

            result.Id.Should().Be(projectId);
        }

        [Fact]
        public async Task GetProjects_ReturnsProjects()
        {
            int projectId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProjectsRepository(context, _mapper);

            var projects = await repository.GetProjects();

            var expectedProjects = await context.Projects.Include(x => x.Author).ToListAsync();

            projects.Should().BeEquivalentTo(expectedProjects, options => options.Excluding(x => x.Products)
                .Excluding(x => x.CompanyId)
                .Excluding(x => x.Company)
                .Excluding(x => x.AuthorId)
                .Excluding(x => x.Author)
                .Excluding(x => x.Id));
        }

        [Fact]
        public async Task CreateProject_ReturnsCreatedProjects()
        {
            var project = new Project {
                Name = "test",
                ClientName = "test",
                Address = "test",
                ImageName = "test",
                CreationDate = "test",
            };

            project.Id = 0;

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProjectsRepository(context, _mapper);

            var cratedProject = await repository.CreateProject(project);

            cratedProject.Name.Should().Be(project.Name);
        }

        [Fact]
        public async Task UpdateProject_UpdatesProject()
        {
            var project = new Project
            {
                Name = "test",
                ClientName = "test",
                Address = "test",
                ImageName = "test",
                CreationDate = "test",
            };

            project.Id = 1;

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProjectsRepository(context, _mapper);

            var updatedProject = await repository.UpdateProject(project);

            updatedProject.Name.Should().Be(project.Name);
            updatedProject.ClientName.Should().Be(project.ClientName);
            updatedProject.ImageName.Should().Be(project.ImageName);
            updatedProject.Address.Should().Be(project.Address);
        }

        [Fact]
        public async Task DeleteProject_DeletesProject()
        {
            // Arrange
            var project = new ProjectEntity
            {
                AuthorId= 1,
                Name = "test",
                ClientName = "test",
                Address = "test",
                ImageName = "test",
                CreationDate = "test",
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new ProjectsRepository(context, _mapper);

            context.Projects.Add(project);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteProject(project.Id);

            // Assert
            var foundEntity = await context.Projects.FirstOrDefaultAsync(x => x.Id == project.Id);
            foundEntity.Should().BeNull();
        }
    }
}
