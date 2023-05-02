using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Company;
using Traki.Infrastructure.Data;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class CompanyControllerTests
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CompanyControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetCompany_ReturnsCompany()
        {
            // Arrange
            int companyId = 1;

            string uri = $"api/companies/{companyId}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsAdministrator();
            var expectedCompany = ExampleData.Companies.First();

            // Act
            var response = await client.Get<GetCompanyResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var company = response.Data.Company;

            company.Id.Should().Be(companyId);
            company.Name.Should().BeEquivalentTo(expectedCompany.Name);
            company.Address.Should().BeEquivalentTo(expectedCompany.Address);
        }

        [Fact]
        public async Task GetCompany_WhenNotAdministrator_ReturnsForbidden()
        {
            // Arrange
            int companyId = 1;

            string uri = $"api/companies/{companyId}";
            var client = _factory.GetCustomHttpClient();
            await client.LoginAsProductManager();
            var expectedCompany = ExampleData.Companies.First();

            // Act
            var response = await client.Get<GetCompanyResponse>(uri);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
