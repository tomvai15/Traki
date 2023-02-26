using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Traki.Api.Contracts.Project;
using Traki.Api.Data;
using Traki.Api.Models.Project;

namespace Traki.IntegrationTests
{
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

            Project expectedResponse = ExampleData.Projects.First();

            string body = await response.Content.ReadAsStringAsync();
            GetProjectResponse getProjectResponse = JsonConvert.DeserializeObject<GetProjectResponse>(body);
            getProjectResponse.Project.Should().BeEquivalentTo(expectedResponse, options => options.Excluding(x => x.Id));
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
    }
}
