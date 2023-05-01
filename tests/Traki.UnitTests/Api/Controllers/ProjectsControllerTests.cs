using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Company;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Contracts.Project;
using Traki.Api.Controllers;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class ProjectsControllerTests
    {
        private readonly Mock<IProjectsRepository> projectsRepository;
        private readonly Mock<IClaimsProvider> claimsProvider;
        private readonly IMapper _mapper;
        private readonly ProjectsController _controller;

        public ProjectsControllerTests()
        {
            projectsRepository = new Mock<IProjectsRepository>();
            claimsProvider = new Mock<IClaimsProvider>();
            _mapper = CreateMapper();
            _controller = new ProjectsController(projectsRepository.Object, claimsProvider.Object, _mapper);
        }

        [Fact]
        public async Task GetProjects()
        {
            // Arrange
            var projectId = 1;
            var items = new List<Project>();
            var response = new GetProjectsResponse
            {
                Projects = _mapper.Map<IEnumerable<ProjectDto>>(items)
            };

            projectsRepository.Setup(repo => repo.GetProjects())
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetProjects();

            // Assert
            var data = result.ShouldBeOfType<GetProjectsResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task GetProject()
        {
            // Arrange
            var projectId = 1;
            var item = new Project();
            var response = new GetProjectResponse
            {
                Project = _mapper.Map<ProjectDto>(item)
            };

            projectsRepository.Setup(repo => repo.GetProject(projectId))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.GetProject(projectId);

            // Assert
            var data = result.ShouldBeOfType<GetProjectResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task PostProject()
        {
            // Arrange
            var productId = 1;
            var drawingId = 1;
            var item = new Project();
            var response = new CreateProjectRequest
            {
                Project = _mapper.Map<ProjectDto>(item)
            };

            projectsRepository.Setup(repo => repo.CreateProject(It.IsAny<Project>()))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.PostProject(response);

            // Assert
            var data = result.ShouldBeOfType<GetProjectResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task UpdateProject()
        {
            // Arrange
            var projectId = 1;
            var item = new Project();
            var response = new CreateProjectRequest
            {
                Project = _mapper.Map<ProjectDto>(item)
            };

            projectsRepository.Setup(repo => repo.CreateProject(It.IsAny<Project>()))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.UpdateProject(projectId, response);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteProject()
        {
            // Arrange
            var projectId = 1;

            projectsRepository.Setup(repo => repo.DeleteProject(projectId));

            // Act
            var result = await _controller.DeleteProject(projectId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
