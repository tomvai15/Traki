using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Recommendation;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class RecommendationsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly TrakiDbContext _context;

        public RecommendationsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            var serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _context = serviceProvider.GetRequiredService<TrakiDbContext>();
        }

        [Fact]
        public async Task GetRecommendations_ReturnsRecommendationsForUser()
        {
            // Arrange
            var url = $"/api/recommendations";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<GetRecommendationResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
