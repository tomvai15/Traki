using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Traki.Api.Contracts.Product;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ProductsControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly TrakiDbContext _context;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            var serviceProvider = factory.Services.CreateScope().ServiceProvider;
            _context = serviceProvider.GetRequiredService<TrakiDbContext>();
        }

        [Fact]
        public async Task GetProduct_ProductExists_ReturnOkAndProductInfromation()
        {
            // Arrange
            var product = _context.Products.Include(x=> x.Author).First();
            var productId = product.Id;
            var projectId = product.ProjectId;
            var url = $"/api/projects/{productId}/products/{projectId}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<GetProductResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetProductResponse getProductResponse = response.Data;
            product.Should().BeEquivalentTo(getProductResponse.Product);
        }

        [Theory]
        [InlineData("/api/projects/1/products/1")]
        [InlineData("/api/projects/1/products")]
        public async Task Get_ReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<ProductDto>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/projects/1/products/1000000")]
        public async Task Get_NotExistingProject_Return404Response(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<ProductDto>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
