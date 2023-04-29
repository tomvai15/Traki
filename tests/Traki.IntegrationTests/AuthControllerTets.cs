using Azure;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Traki.Api.Contracts.Auth;
using Traki.IntegrationTests.Extensions;

namespace Traki.IntegrationTests
{
    [Collection("Sequential")]
    public class AuthControllerTets
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string baseUri = "api/auth";

        public AuthControllerTets(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task LoginJwt_ValidCredentials_RetursAccessAndRefreshToken()
        {
            // Arrange
            string uri = baseUri + "/jwt-login";
            var client = _factory.GetCustomHttpClient();

            var loginRequest = new LoginRequest
            {
                Email = "vainoristomas@gmail.com",
                Password = "password"
            };

            // Act
            var response = await client.Post<LoginRequest, LoginResponse>(uri, loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Data.Email.Should().Be(loginRequest.Email);
            response.Data.Token.Should().NotBeNullOrEmpty();
            response.Data.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginJwt_NotValidCredentials_RetursForbidden()
        {
            // Arrange
            string uri = baseUri + "/jwt-login";
            var client = _factory.GetCustomHttpClient();

            var loginRequest = new LoginRequest
            {
                Email = Any<string>(),
                Password = Any<string>()
            };

            // Act
            var response = await client.Post<LoginRequest, LoginResponse>(uri, loginRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
