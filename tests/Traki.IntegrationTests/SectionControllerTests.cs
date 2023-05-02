using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Auth;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.Section;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class SectionControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string baseUri = "api/auth";

        public SectionControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetSections_ReturnsProtocolSections()
        {
            // Arrange
            int protocolId = 1;
            int sectionId = 1;

            string uri = $"api/protocols/{protocolId}/sections";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();
            var expectedSections = ExampleData.Sections.Where(x => x.ProtocolId == protocolId);

            // Act
            var response = await client.Get<GetSectionsResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var sections = response.Data.Sections;

            sections.Should().BeEquivalentTo(expectedSections, options => options.Excluding(x => x.Checklist)
                .Excluding(x => x.Table)
                .Excluding(x => x.Protocol)
                .Excluding(x => x.Id));

        }

        [Fact]
        public async Task GetSection_ReturnsSection()
        {
            // Arrange
            int protocolId = 1;
            int sectionId = 1;

            string uri = $"api/protocols/{protocolId}/sections/{sectionId}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();
            var expectedSection = ExampleData.Sections.First();

            // Act
            var response = await client.Get<GetSectionResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var section = response.Data.Section;

            section.Id.Should().Be(sectionId);
            section.Name.Should().BeEquivalentTo(expectedSection.Name);
            section.Priority.Should().Be(expectedSection.Priority);
        }
    }
}
