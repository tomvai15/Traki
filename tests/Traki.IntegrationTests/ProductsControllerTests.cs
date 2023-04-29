using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/projects/1/products/1")]
        public async Task Get_ExistingProject_ReturnSuccess(string url)
        {
            // Arrange
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();

            // Act
            var response = await client.Get<GetProductResponse>(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var expectedResponse = ExampleData.Products.First();

            GetProductResponse getProductResponse = response.Data;

            expectedResponse.Should().BeEquivalentTo(getProductResponse.Product,
                options => options.Excluding(x => x.Author)
                                  .Excluding(x=> x.Id));
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
