using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Auth;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.Protocol;
using Traki.Api.Contracts.Section;
using Traki.Api.Contracts.Template;
using Traki.Domain.Models;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ProtocolsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string baseUri = "api/auth";

        public ProtocolsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetProtocolTemplates_ReturnsProtocolTemplates()
        {
            // Arrange
            int protocolId = 1;
            int sectionId = 1;

            string uri = $"api/protocols/templates";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();
            var expectedProtocols = ExampleData.Protocols.Where(x => x.IsTemplate == true);

            // Act
            var response = await client.Get<GetProtocolsResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var protocols = response.Data.Protocols;

            protocols.Should().BeEquivalentTo(expectedProtocols, options => options.Excluding(x => x.Product)
                .Excluding(x => x.Sections)
                .Excluding(x => x.Signer)
                .Excluding(x => x.EnvelopeId)
                .Excluding(x => x.ProductId)
                .Excluding(x => x.CreationDate)
                .Excluding(x => x.Id));

        }

        [Fact]
        public async Task GetProtocol_ReturnsProtocol()
        {
            // Arrange
            int protocolId = 1;

            string uri = $"api/protocols/{protocolId}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();
            var expectedProtocol = ExampleData.Protocols.First();

            // Act
            var response = await client.Get<GetProtocolResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var protocol = response.Data.Protocol;

            protocol.Should().BeEquivalentTo(expectedProtocol, options => options.Excluding(x => x.Product)
                .Excluding(x => x.Sections)
                .Excluding(x => x.Signer)
                .Excluding(x => x.EnvelopeId)
                .Excluding(x => x.ProductId)
                .Excluding(x => x.CreationDate)
                .Excluding(x => x.Id));
        }
    }
}
