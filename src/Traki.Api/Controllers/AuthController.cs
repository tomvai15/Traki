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

        public AuthController(IDocuSignService docuSignService, IJwtTokenGenerator jwtTokenGenerator, IAccessTokenProvider accessTokenProvider, IUserAuthHandler authHandler)
        {
            _authHandler = authHandler;
            _docuSignService = docuSignService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _accessTokenProvider = accessTokenProvider;
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

            string accessToken;
            try
            {
                accessToken = await _accessTokenProvider.GetAccessToken();
            }
            catch
            {
                accessToken = null;
            }

            var response = new GetUserResponse { User = new UserDto { Id = userId }, LoggedInDocuSign = accessToken.IsNullOrEmpty() };
            return Ok(response);
        }

        [HttpPost("code")]
        [Authorize]
        public async Task<ActionResult> GetAuthorisationCodeRequestUrl([FromBody] AuthorisationCodeRequest getAuthorisationCodeRequest)
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
