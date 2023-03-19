using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Traki.Api.Constants;
using Traki.Api.Contracts;
using Traki.Api.Cryptography;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserAuthHandler _authHandler;

        public AuthController(IJwtTokenGenerator jwtTokenGenerator, IUserAuthHandler authHandler)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _authHandler = authHandler;
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
        public async Task<ActionResult> GetUserInfo()
        {
            string name = User.Claims.FirstOrDefault(x => x.Type == Claims.UserId).Value;
            return Ok(new { name = name });
        }
    }
}
