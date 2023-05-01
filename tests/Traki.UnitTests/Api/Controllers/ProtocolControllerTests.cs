using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Company;
using Traki.Api.Contracts.Drawing;
using Traki.Api.Contracts.Project;
using Traki.Api.Contracts.Protocol;
using Traki.Api.Controllers;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class ProtocolControllerTests
    {
        private readonly Mock<IProtocolRepository> protocolRepository;
        private readonly Mock<ISectionHandler> sectionHandler;
        private readonly IMapper _mapper;
        private readonly ProtocolsController _controller;

        public ProtocolControllerTests()
        {
            protocolRepository = new Mock<IProtocolRepository>();
            sectionHandler = new Mock<ISectionHandler>();
            _mapper = CreateMapper();
            _controller = new ProtocolsController(protocolRepository.Object, sectionHandler.Object, _mapper);
        }


        [Fact]
        public async Task GetProtocol()
        {
            // Arrange
            var protocolId = 1;
            var item = new Protocol();
            var response = new GetProtocolResponse
            {
                Protocol = _mapper.Map<ProtocolDto>(item)
            };

            protocolRepository.Setup(repo => repo.GetProtocol(protocolId))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.GetProtocol(protocolId);

            // Assert
            var data = result.ShouldBeOfType<GetProtocolResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task GetTemplateProtocols()
        {
            // Arrange
            var projectId = 1;
            var items = new List<Protocol>();
            var response = new GetProtocolsResponse
            {
                Protocols = _mapper.Map<IEnumerable<ProtocolDto>>(items)
            };

            protocolRepository.Setup(repo => repo.GetTemplateProtocols())
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetTemplateProtocols();

            // Assert
            var data = result.ShouldBeOfType<GetProtocolsResponse>();
            response.Should().BeEquivalentTo(data);
        }


        [Fact]
        public async Task CreateProtocol()
        {
            // Arrange
            var productId = 1;
            var drawingId = 1;
            var item = new Protocol();
            var request = new CreateProtocolRequest
            {
                Protocol = _mapper.Map<ProtocolDto>(item)
            };

            protocolRepository.Setup(repo => repo.CreateProtocol(It.IsAny<Protocol>()))
                .ReturnsAsync(item);

            // Act
            var result = await _controller.CreateProtocol(request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateProtocol()
        {
            // Arrange
            var protocolId = 1;
            var drawingId = 1;
            var item = new Protocol();
            var request = new UpdateProtocolRequest
            {
                Protocol = _mapper.Map<ProtocolDto>(item)
            };

            protocolRepository.Setup(repo => repo.UpdateProtocol(It.IsAny<Protocol>()));

            // Act
            var result = await _controller.UpdateProtocol(protocolId, request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }


        [Fact]
        public async Task DeleteProject()
        {
            // Arrange
            var protocolId = 1;

            protocolRepository.Setup(repo => repo.DeleteProtocol(protocolId));

            // Act
            var result = await _controller.DeleteProtocol(protocolId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        /*

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
        }*/
    }
}
