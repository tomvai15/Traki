using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Traki.Api.Contracts.Auth;
using Traki.Api.Controllers;
using Traki.Domain.Cryptography;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Providers;
using Traki.Domain.Services.Docusign;
using static Traki.UnitTests.Helpers.Dummy;

namespace Traki.UnitTests.Api.Controllers
{
    /*public class AuthControllerTests
    {
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        private readonly Mock<IUserAuthHandler> _authHandler = new Mock<IUserAuthHandler>();
        private readonly Mock<IDocuSignService> _docuSignService = new Mock<IDocuSignService>();
        private readonly Mock<IAccessTokenProvider> _accessTokenProvider = new Mock<IAccessTokenProvider>();

        private readonly AuthController authController;

        public AuthControllerTests()
        {
            authController = new AuthController(_docuSignService.Object, _jwtTokenGenerator.Object, _accessTokenProvider.Object, _authHandler.Object);
        }
        
        
        [Fact]
        public async void JWTLogin_NoError_ReturnsJWTToken()
        {
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string token = Any<string>();

            _authHandler.Setup(x => x.GetUser(loginRequest.Email, loginRequest.Password)).ReturnsAsync(user);
            _authHandler.Setup(x => x.CreateClaimsForUser(It.IsAny<User>())).ReturnsAsync(claims);
            _jwtTokenGenerator.Setup(x => x.GenerateJWTToken(It.IsAny<IEnumerable<Claim>>())).Returns(token);

            ActionResult<LoginResponse> response = await authController.JWTLogin(loginRequest);

            OkObjectResult result = (OkObjectResult)response.Result;
            var a = (LoginResponse)result.Value;

            a.Token.Should().Be(token);
            a.Email.Should().Be(loginRequest.Email);
        }
    }*/
}
