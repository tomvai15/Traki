using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Report;
using Traki.Domain.Services.BlobStorage;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ImageControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly IServiceProvider _serviceProvider;
        private readonly TrakiDbContext _context;

        public ImageControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _context = _serviceProvider.GetRequiredService<TrakiDbContext>();
        }
        [Fact]
        public async Task GetImage_ImageExists_ReturnsImage()
        {
            // Arrange
            var result = AddReportForProtocol();
            var imageName = result.Item1;
            var expectedContent = result.Item2;
            string folderName = "company";
            var url = $"/api/control/folders/{folderName}/files/{imageName}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.SendGetRequest(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Headers.ContentType.MediaType.Should().Be("image/png");

            var content = await response.Content.ReadAsByteArrayAsync();
            content.Should().BeEquivalentTo(expectedContent);
        }

        [Fact]
        public async Task GetImage_ImageDoesNotExist_Returns404()
        {
            // Arrange
            string imageName = Any<string>() + ".png";
            string folderName = "company";
            var url = $"/api/control/folders/{folderName}/files/{imageName}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.SendGetRequest(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetImage_UnauthorisedUser_Returns401()
        {
            // Arrange
            string imageName = Any<string>() + ".png";
            string folderName = "company";
            var url = $"/api/control/folders/{folderName}/files/{imageName}";
            var client = _factory.GetCustomHttpClient();

            // Act
            var response = await client.SendGetRequest(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        private (string, byte[]) AddReportForProtocol()
        {
            string imageName = Any<string>() + ".png";
            var storageService = _serviceProvider.GetRequiredService<IStorageService>();

            var content = AnyMany<byte>(3).ToArray();
            var memoryStream = new MemoryStream(content);
            storageService.AddFile("company", imageName, "image/png", memoryStream);

            return (imageName, content);
        }
    }
}
