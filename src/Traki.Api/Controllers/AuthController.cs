using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Traki.Api.Contracts.Auth;
using Traki.Api.Contracts.User;
using Traki.Domain.Constants;
using Traki.Domain.Cryptography;
using Traki.Domain.Exceptions;
using Traki.Domain.Handlers;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Docusign;

namespace Traki.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserAuthHandler _authHandler;
        private readonly IDocuSignService _docuSignService;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IUsersRepository _usersRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IMapper _mapper;

        public AuthController(IMapper mapper, IClaimsProvider claimsProvider, IDocuSignService docuSignService, IJwtTokenGenerator jwtTokenGenerator, IAccessTokenProvider accessTokenProvider, IUserAuthHandler authHandler, IUsersRepository usersRepository)
        {
            _authHandler = authHandler;
            _docuSignService = docuSignService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _accessTokenProvider = accessTokenProvider;
            _usersRepository = usersRepository;
            _claimsProvider = claimsProvider;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _authHandler.GetUser(loginRequest.Email, loginRequest.Password);

            var claims = await _authHandler.CreateClaimsForUser(user);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            return Ok();
        }

        [HttpPost("activate")]
        public async Task<ActionResult> ActivateAccount([FromBody] ActivateAccountRequest request)
        {
            await _authHandler.ActivateAccount(request.RegisterId, request.Code, request.Password);
            return Ok();
        }

        [HttpPost("jwt-login")]
        public async Task<ActionResult<LoginResponse>> JWTLogin([FromBody] LoginRequest loginRequest)
        {
            var user = await _authHandler.GetUser(loginRequest.Email, loginRequest.Password);

            var claims = await _authHandler.CreateClaimsForUser(user);

            var token = _jwtTokenGenerator.GenerateJWTToken(claims);

            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _usersRepository.UpdateUser(user);

            return Ok(new LoginResponse { 
                Email = loginRequest.Email, 
                Token = token,
                RefreshToken = refreshToken,
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<LoginResponse>> RefreshJwtToken([FromBody] RefreshTokenRequest request)
        {
            var userId = _jwtTokenGenerator.GetPrincipalFromExpiredToken(request.Token);

            var user = await _usersRepository.GetUserById(userId);

            if (user.RefreshTokenExpiryTime < DateTime.Now || user.RefreshToken != request.RefreshToken)
            {
                throw new UnauthorizedException();
            }

            var claims = await _authHandler.CreateClaimsForUser(user);

            var token = _jwtTokenGenerator.GenerateJWTToken(claims);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _usersRepository.UpdateUser(user);

            return Ok(new LoginResponse { 
                Email = user.Email, 
                Token = token,
                RefreshToken = refreshToken
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpGet("userstate")]
        [Authorize]
        public async Task<ActionResult<GetUserStateResponse>> GetUserInfo()
        {
            int userId = GetUserId();

            string accessToken;
            try
            {
                accessToken = await _accessTokenProvider.GetAccessToken();
            }
            catch
            {
                accessToken = null;
            }

            var user = await _usersRepository.GetUserById(userId);

            var response = new GetUserStateResponse
            {
                User = new UserInfoDto
                {
                    Id = userId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,  
                },
                LoggedInDocuSign = !accessToken.IsNullOrEmpty()
            };
            return Ok(response);
        }

        [HttpGet("userinfo")]
        [Authorize]
        public async Task<ActionResult<GetUserInfoResponse>> GetFullUserInfo()
        {
            int userId = GetUserId();
            var user = await _usersRepository.GetUserById(userId);

            var response = new GetUserInfoResponse
            {
                User = _mapper.Map<UserDto>(user),
            };
            return Ok(response);
        }

        [HttpPost("userinfo")]
        [Authorize]
        public async Task<ActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest request)
        {
            int userId = GetUserId();
            var user = await _usersRepository.GetUserById(userId);      
            user.UserIconBase64 = request.User.UserIconBase64;

            await _usersRepository.UpdateUser(user);
            return Ok();
        }

        [HttpPost("registerdevice")]
        [Authorize]
        public async Task<ActionResult> GetAuthorisationCodeRequestUrl([FromBody] RegisterDeviceRequest request)
        {
            _claimsProvider.TryGetUserId(out int userId);
            var user = await _usersRepository.GetUserById(userId);

            user.DeviceToken = request.DeviceToken;
            await _usersRepository.UpdateUser(user);
            return Ok();
        }

        [HttpPost("code")]
        [Authorize]
        public async Task<ActionResult> RegisterDevice([FromBody] AuthorisationCodeRequest getAuthorisationCodeRequest)
        {
            var url = await _docuSignService.GetAuthorisationCodeRequest(getAuthorisationCodeRequest.State);
            return Ok(url);
        }

        [HttpPost("docusign")]
        [Authorize]
        public async Task<ActionResult> LoginToDocuSign([FromBody] LoginOAuthRequest loginOAuthRequest)
        {
            var oauthResponse = await _docuSignService.GetAccessTokenUsingCode(loginOAuthRequest.Code);

            await _accessTokenProvider.AddAccessToken(oauthResponse);

            // create new cookie?
            return Ok();
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == Claims.UserId).Value);
        }
    }
}
