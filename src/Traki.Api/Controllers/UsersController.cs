using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts;
using Traki.Api.Handlers;

namespace Traki.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserAuthHandler _userAuthHandler;

        public UsersController(IUserAuthHandler userAuthHandler)
        {
            _userAuthHandler = userAuthHandler;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            bool isUserAdded = await _userAuthHandler.TryCreateUser(createUserRequest);

            if (!isUserAdded)
            {
                return BadRequest(new { message = "Email is already taken" });
            }
            return NoContent();
        }

        [HttpPost("token")]
        public async Task<ActionResult<LoginResponse>> GetCredentials(LoginRequest loginRequest)
        {
            try
            {
                LoginResponse loginResponse = await _userAuthHandler.LoginUser(loginRequest);

                if (loginResponse == null)
                {
                    return Unauthorized();
                }

                return Ok(loginResponse);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
