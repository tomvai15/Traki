using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Security.Policy;
using Traki.Api.Contracts.Project;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProjectsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CreateProject_ValidFields_ReturnsOk()
        {
            // Arrange
            string url = "/api/projects";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProjectManager();
            var request = new CreateProjectRequest
            {
                Project = new ProjectDto
                {
                    Name = "Name",
                    ClientName = "client",
                    Address = "address",
                    ImageName= "ima.ge.png",
                    CreationDate = ""
                }
            };

            // Act
            var response = await client.Post<CreateProjectRequest, GetProjectResponse>(url, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateProject_InvalidFields_ReturnsOk()
        {
            // Arrange
            string url = "/api/projects";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProjectManager();
            var request = new CreateProjectRequest
            {
                Project = new ProjectDto
                {
                    Name = "<><>",
                    ClientName = "<><>",
                    Address = "<><>",
                    ImageName = "<><>",
                }
            };

            // Act
            var response = await client.Post<CreateProjectRequest, GetProjectResponse>(url, request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("/api/projects/1")]
        public async Task Get_ExistingProject_ReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();
            await client.AddJwtToken("vainoristomas@gmail.com", "password");

            // Act
            var response = await client.Get<GetProjectResponse>(url);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var expectedResponse = ExampleData.Projects.First();

            GetProjectResponse getProjectResponse = response.Data;
            getProjectResponse.Project.Should().BeEquivalentTo(expectedResponse,
                options => options.Excluding(x => x.Products)
                .Excluding(x => x.CompanyId)
                .Excluding(x => x.Company)
                .Excluding(x => x.AuthorId)
                .Excluding(x => x.CreationDate)
                .Excluding(x => x.Author)
                .Excluding(x => x.Templates)
                .Excluding(x => x.Id));
        }

        [Theory]
        [InlineData("/api/projects/1")]
        [InlineData("/api/projects")]
        public async Task Get_ReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();
            await client.AddJwtToken("vainoristomas@gmail.com", "password");

            // Act
            var response = await client.Get<GetProjectResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/projects/10001")]
        public async Task Get_NotExistingProject_Return404Response(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();
            await client.AddJwtToken("vainoristomas@gmail.com", "password");

            // Act
            var response = await client.Get<GetProjectResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("/api/projects/1")]
        public async Task Get_UnauthorizedUser_Return401(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();

            // Act
            var response = await client.Get<GetProjectResponse>(url);

            // Assert

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
