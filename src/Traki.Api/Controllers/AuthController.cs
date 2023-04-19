using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Traki.Api.Contracts.Auth;
using Traki.Domain.Constants;
using Traki.Domain.Cryptography;
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

        public AuthController(IClaimsProvider claimsProvider, IDocuSignService docuSignService, IJwtTokenGenerator jwtTokenGenerator, IAccessTokenProvider accessTokenProvider, IUserAuthHandler authHandler, IUsersRepository usersRepository)
        {
            _authHandler = authHandler;
            _docuSignService = docuSignService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _accessTokenProvider = accessTokenProvider;
            _usersRepository = usersRepository;
            _claimsProvider = claimsProvider;
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

        [HttpPost("jwtlogin")]
        public async Task<ActionResult<LoginResponse>> JWTLogin([FromBody] LoginRequest loginRequest)
        {
            var user = await _authHandler.GetUser(loginRequest.Email, loginRequest.Password);

            var claims = await _authHandler.CreateClaimsForUser(user);

            var token = _jwtTokenGenerator.GenerateJWTToken(claims);

            return Ok(new LoginResponse { Email = loginRequest.Email, Token = token });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Ok();
        }

        [HttpGet("userinfo")]
        [Authorize]
        public async Task<ActionResult<GetUserInfoResponse>> GetUserInfo()
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

            var response = new GetUserInfoResponse { 
                User = new UserInfoDto { 
                    Id = userId, 
                    Name = user.Name,
                    Email = user.Email,
                },
                LoggedInDocuSign = !accessToken.IsNullOrEmpty() 
            };
            return Ok(response);
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
