using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class DefectControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string baseUri = "api/auth";

        public DefectControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetDefect_RetursnDefect()
        {
            // Arrange
            int drawingId = 1;
            int defectId = 1;
            string uri = $"api/drawings/{drawingId}/defects/{defectId}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();
            var expectedDefect = ExampleData.Defects.First();

            // Act
            var response = await client.Get<GetDefectResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var defect = response.Data.Defect;

            defect.Id.Should().Be(defectId);
            defect.Title.Should().BeEquivalentTo(expectedDefect.Title);
            defect.Description.Should().BeEquivalentTo(expectedDefect.Description);
            defect.ImageName.Should().BeEquivalentTo(expectedDefect.ImageName);
        }    
    }
}
