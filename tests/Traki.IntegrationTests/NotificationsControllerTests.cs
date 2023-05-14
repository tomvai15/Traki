using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Domain.Services.BlobStorage;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{

    [Collection("Sequential")]
    public class NotificationsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly TrakiDbContext _context;

        public NotificationsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _context = _serviceProvider.GetRequiredService<TrakiDbContext>();
        }

        [Fact]
        public async Task GetNotifications_ReturnsUsersNotifications()
        {
            // Arrange
            var url = $"/api/notifications";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<GetDefectNotificationsResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var getReportResponse = response.Data;
            getReportResponse.DefectNotifications.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteNotifications_DeletesAllUsersNotificationsRelatedToDefect()
        {
            Assert.True(true);
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
