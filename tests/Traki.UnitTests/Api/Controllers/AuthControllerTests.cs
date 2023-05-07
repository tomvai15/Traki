using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Traki.Api.Contracts.Auth;
using Traki.Api.Contracts.Project;
using Traki.Api.Contracts.User;
using Traki.Api.Controllers;
using Traki.Domain.Cryptography;
using Traki.Domain.Exceptions;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services;
using Traki.Domain.Services.Docusign;
using Traki.UnitTests.Helpers;
using static Traki.UnitTests.Helpers.Dummy;
using User = Traki.Domain.Models.User;

namespace Traki.UnitTests.Api.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        private readonly Mock<IUserAuthHandler> _authHandler = new Mock<IUserAuthHandler>();
        private readonly Mock<IDocuSignService> _docuSignService = new Mock<IDocuSignService>();
        private readonly Mock<IAccessTokenProvider> _accessTokenProvider = new Mock<IAccessTokenProvider>();
        private readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        private readonly Mock<IClaimsProvider> _claimsProvider = new Mock<IClaimsProvider>();
        private readonly Mock<ControllerContext> mockControllerContext = new Mock<ControllerContext>();
        private readonly IMapper _mapper = CreateMapper();

        private readonly AuthController authController;

        public AuthControllerTests()
        {
            authController = new AuthController(
                _mapper,
                _claimsProvider.Object,
                _docuSignService.Object, 
                _jwtTokenGenerator.Object, 
                _accessTokenProvider.Object, 
                _authHandler.Object,
                _usersRepository.Object);

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            authServiceMock
                .Setup(_ => _.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            authController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    // How mock RequestServices?
                    RequestServices = serviceProviderMock.Object
                }
            };
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

        [Fact]
        public async void Login()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string token = Any<string>();

            _authHandler.Setup(x => x.GetUser(loginRequest.Email, loginRequest.Password)).ReturnsAsync(user);
            _authHandler.Setup(x => x.CreateClaimsForUser(It.IsAny<User>())).ReturnsAsync(claims);

            // Act
            var response = await authController.Login(loginRequest);

            // Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void Logout()
        {
            // Act
            var response = await authController.LogOut();

            // Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async void GetFullUserInfo()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string token = Any<string>();

            var response = new GetUserInfoResponse
            {
                User = _mapper.Map<UserDto>(user)
            };

            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await authController.GetFullUserInfo();

            // Assert
            var data = result.ShouldBeOfType<GetUserInfoResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async void GetUserInfo_AccesTokenExists()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string accessToken = Any<string>();
            int userId = 1;

            var response = new GetUserStateResponse
            {
                User = new UserInfoDto
                {
                    Id = userId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                },
                LoggedInDocuSign = true
            };

            _claimsProvider.Setup(s => s.TryGetUserId(out userId));
            _accessTokenProvider.Setup(x => x.GetAccessToken()).ReturnsAsync(accessToken);
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await authController.GetUserInfo();

            // Assert
            var data = result.ShouldBeOfType<GetUserStateResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async void RefreshJwtToken()
        {
            // Arrange
            var refreshTokenRequest = Any<RefreshTokenRequest>();

            var user = Any<User>();
            user.RefreshToken = refreshTokenRequest.RefreshToken;   
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            var claims = new Claim[0];
            string accessToken = Any<string>();
            string refreshToken = Any<string>();
            int userId = 1;

            var request = Any<RefreshTokenRequest>();

            _jwtTokenGenerator.Setup(s => s.GetPrincipalFromExpiredToken(request.Token)).Returns(userId);
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);
            _authHandler.Setup(x => x.CreateClaimsForUser(It.IsAny<User>())).ReturnsAsync(claims);

            _jwtTokenGenerator.Setup(x => x.GenerateJWTToken(claims)).Returns(accessToken);
            _jwtTokenGenerator.Setup(x => x.GenerateRefreshToken()).Returns(refreshToken);

            // Act
            var result = await authController.RefreshJwtToken(refreshTokenRequest);

            // Assert
            var data = result.ShouldBeOfType<LoginResponse>();
            data.Token.Should().BeEquivalentTo(accessToken);
            data.RefreshToken.Should().BeEquivalentTo(refreshToken);

            _usersRepository.Verify(x => x.UpdateUser(user));
        }

        [Fact]
        public async void RefreshJwtToken_IncorrectRefreshToken()
        {
            // Arrange
            var refreshTokenRequest = Any<RefreshTokenRequest>();

            var user = Any<User>();
            user.RefreshToken = Any<string>();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            var claims = new Claim[0];
            string accessToken = Any<string>();
            string refreshToken = Any<string>();
            int userId = 1;

            var request = Any<RefreshTokenRequest>();

            _jwtTokenGenerator.Setup(s => s.GetPrincipalFromExpiredToken(request.Token)).Returns(userId);
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            Func<Task<ActionResult<LoginResponse>>> refreshTokenFunc = async () => await authController.RefreshJwtToken(refreshTokenRequest);

            // Assert
            await refreshTokenFunc.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async void RefreshJwtToken_ExpiredRefreshToken()
        {
            // Arrange
            var refreshTokenRequest = Any<RefreshTokenRequest>();

            var user = Any<User>();
            user.RefreshToken = Any<string>();
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(-1);
            var claims = new Claim[0];
            string accessToken = Any<string>();
            string refreshToken = Any<string>();
            int userId = 1;

            var request = Any<RefreshTokenRequest>();

            _jwtTokenGenerator.Setup(s => s.GetPrincipalFromExpiredToken(request.Token)).Returns(userId);
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            Func<Task<ActionResult<LoginResponse>>> refreshTokenFunc = async () => await authController.RefreshJwtToken(refreshTokenRequest);

            // Assert
            await refreshTokenFunc.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async void GetUserInfo_AccesTokenDoesNotExists()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string accessToken = Any<string>();
            int userId = 1;

            var response = new GetUserStateResponse
            {
                User = new UserInfoDto
                {
                    Id = userId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                },
                LoggedInDocuSign = false
            };

            _claimsProvider.Setup(s => s.TryGetUserId(out userId));
            _accessTokenProvider.Setup(x => x.GetAccessToken()).ThrowsAsync(new ArgumentException());
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await authController.GetUserInfo();

            // Assert
            var data = result.ShouldBeOfType<GetUserStateResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task JwtLogOut()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string accessToken = Any<string>();
            int userId = 1;

            _claimsProvider.Setup(s => s.TryGetUserId(out userId));
            _accessTokenProvider.Setup(x => x.GetAccessToken()).ReturnsAsync(accessToken);
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);

            // Act
            var result = await authController.JwtLogOut();

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task UpdateUserInfo()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string accessToken = Any<string>();
            int userId = 1;

            var request = new UpdateUserInfoRequest
            {
                User = _mapper.Map<UserDto>(user)
            };

            _claimsProvider.Setup(s => s.TryGetUserId(out userId));
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);
            _usersRepository.Setup(x => x.UpdateUser(user));

            // Act
            var result = await authController.UpdateUserInfo(request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RegiisterDevice()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var claims = new Claim[0];
            string deviceToken = Any<string>();
            int userId = 1;

            var request = new RegisterDeviceRequest
            {
                DeviceToken = deviceToken
            };

            _claimsProvider.Setup(s => s.TryGetUserId(out userId));
            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(user);
            _usersRepository.Setup(x => x.UpdateUser(user));

            // Act
            var result = await authController.RegisterDevice(request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task LoginToDocuSign()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var authResponse = Any<OAuthResponse>();
            var claims = new Claim[0];
            string code = Any<string>();
            int userId = 1;

            var request = new LoginOAuthRequest
            {
                Code = code
            };

            _docuSignService.Setup(s => s.GetAccessTokenUsingCode(code)).ReturnsAsync(authResponse);

            // Act
            var result = await authController.LoginToDocuSign(request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _accessTokenProvider.Verify(x => x.AddAccessToken(authResponse));
        }


        [Fact]
        public async Task GetAuthorisationCode()
        {
            // Arrange
            var loginRequest = Any<LoginRequest>();

            var user = Any<User>();
            var authResponse = Any<OAuthResponse>();
            var claims = new Claim[0];
            string state = Any<string>();
            string url = Any<string>();
            int userId = 1;

            var request = new AuthorisationCodeRequest
            {
                State = state
            };

            _docuSignService.Setup(s => s.GetAuthorisationCodeRequest(state)).ReturnsAsync(url);

            // Act
            var result = await authController.GetAuthRedirectUrl(request);

            // Assert
            var data = result.ShouldBeOfType<string>();
            data.Should().BeEquivalentTo(url);
        }

    }
}
