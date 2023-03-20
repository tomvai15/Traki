using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using Traki.Api.Constants;
using Traki.Api.Contracts.Auth;
using Traki.Api.Cryptography;
using Traki.Api.Handlers;
using Traki.Api.Services.Docusign;

namespace Traki.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserAuthHandler _authHandler;
        private readonly IDocuSignService _docuSignService;
        private readonly IMemoryCache _memoryCache;

        public AuthController(IDocuSignService docuSignService, IJwtTokenGenerator jwtTokenGenerator, IMemoryCache memoryCache, IUserAuthHandler authHandler)
        {
            _authHandler = authHandler;
            _docuSignService = docuSignService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _memoryCache = memoryCache;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _authHandler.GetUser(loginRequest.Email, loginRequest.Password);

            var claims = await _authHandler.CrateClaimsForUser(user);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
            return Ok();
        }

        [HttpPost("jwtlogin")]
        public async Task<ActionResult<LoginResponse>> JWTLogin([FromBody] LoginRequest loginRequest)
        {
            var user = await _authHandler.GetUser(loginRequest.Email, loginRequest.Password);

            var claims = await _authHandler.CrateClaimsForUser(user);

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
        public async Task<ActionResult<GetUserResponse>> GetUserInfo()
        {
            int userId = GetUserId();

            bool exists = _memoryCache.TryGetValue<string>(GetUserId(), out string accessToken);

            var response = new GetUserResponse { User = new UserDto { Id = userId }, LoggedInDocuSign = exists };
            return Ok(response);
        }

        [HttpGet("code")]
        [Authorize]
        public async Task<ActionResult> GetAuthorisationCodeRequestUrl()
        {
            var url = await _docuSignService.GetAuthorisationCodeRequest();
            return Ok(url);
        }

        [HttpPost("docusign")]
        [Authorize]
        public async Task<ActionResult> LoginToDocuSign([FromBody] LoginOAuthRequest loginOAuthRequest)
        {
            var oauthResponse = await _docuSignService.GetAccessToken(loginOAuthRequest.Code);
            int userId = GetUserId();

            _memoryCache.Set(userId, oauthResponse.AccessToken);

            // create new cookie?
            return Ok();
        }

        private int GetUserId()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == Claims.UserId).Value);
        }
    }
}
