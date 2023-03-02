﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using Traki.Api.Contracts.Product;
using Traki.Api.Contracts.Project;
using Traki.Api.Data;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class ProductsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/products/1")]
        public async Task Get_ExistingProject_ReturnSuccess(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();

            var expectedResponse = ExampleData.Products.First();

            string body = await response.Content.ReadAsStringAsync();
            GetProductResponse getProductResponse = JsonConvert.DeserializeObject<GetProductResponse>(body);
            getProductResponse.Product.Should().BeEquivalentTo(expectedResponse, 
                options => options.Excluding(x => x.Project).Excluding(x=> x.Id));
        }

        [Theory]
        [InlineData("/api/products/1")]
        [InlineData("/api/products")]
        public async Task Get_ReturnSuccess(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
        }

        [Theory]
        [InlineData("/api/products/10001")]
        public async Task Get_NotExistingProject_Return404Response(string url)
        {
            // Arrange
            HttpClient client = _factory.CreateClient();

            // Act
            HttpResponseMessage response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}