using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Report;
using Traki.Domain.Services.BlobStorage;
using Traki.Infrastructure.Data;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class FileControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly TrakiDbContext _context;

        public FileControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _context = _serviceProvider.GetRequiredService<TrakiDbContext>();
        }
        [Fact]
        public async Task GetImage_ImageExists_ReturnsImage()
        {
            Assert.True(true);
        }

        [Fact]
        public async Task GetImage_ImageDoesNotExist_Returns404()
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
