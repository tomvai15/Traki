using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using Traki.Api.Contracts.Project;
using Traki.Api.Data;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ProjectsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProjectsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/projects/1")]
        public async Task Get_ExistingProject_ReturnSuccess(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var expectedResponse = ExampleData.Projects.First();

            string body = await response.Content.ReadAsStringAsync();
            GetProjectResponse getProjectResponse = JsonConvert.DeserializeObject<GetProjectResponse>(body);
            getProjectResponse.Project.Should().BeEquivalentTo(expectedResponse, 
                options => options.Excluding(x => x.Products).Excluding(x=> x.Id));
        }

        [Theory]
        [InlineData("/api/projects/1")]
        [InlineData("/api/projects")]
        public async Task Get_ReturnSuccess(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Theory]
        [InlineData("/api/projects/10001")]
        public async Task Get_NotExistingProject_Return404Response(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
