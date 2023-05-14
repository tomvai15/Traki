using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Report;
using Traki.Domain.Services.BlobStorage;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ReportsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly TrakiDbContext _context;

        public ReportsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _context = _serviceProvider.GetRequiredService<TrakiDbContext>();
        }

        [Fact]
        public async Task GetReport_ReportExist_ReturnsReport()
        {
            // Arrange
            var result = AddReportForProtocol();
            var protocolId = result.Item1;
            var url = $"/api/protocols/{protocolId}/reports";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<GetReportResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var getReportResponse = response.Data;
            getReportResponse.Exists.Should().BeTrue();
        }

        [Fact]
        public async Task GetReport_ReportDoesNotExist_DoesNotReturnReport()
        {
            // Arrange
            var protocol = _context.Protocols.First();
            var protocolId = protocol.Id;
            var url = $"/api/protocols/{protocolId}/reports";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<GetReportResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var getProductResponse = response.Data;
            getProductResponse.Exists.Should().BeFalse();
        }

        private (int, byte[]) AddReportForProtocol()
        {
            string reportName = Any<string>() + ".pdf";
            var protocol = ExampleData.Protocols.First();
            protocol.ReportName = reportName;
            _context.Protocols.Add(protocol);
            _context.SaveChanges();
            int protocolId = protocol.Id;
            var storageService = _serviceProvider.GetRequiredService<IStorageService>();

            var content = AnyMany<byte>(3).ToArray();
            var memoryStream = new MemoryStream(content);
            storageService.AddFile("company", reportName, "application/pdf", memoryStream);

            return (protocolId, content);
        }
    }
}
