using Inventory.Domain.UserAuthentication;
using Inventory.Domain.UserAuthentication.Login;
using Microsoft;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserAuthHandler userHandler;

        public UserController(IUserAuthHandler userHandler)
        {
            Requires.NotNull(userHandler, nameof(userHandler));

            this.userHandler = userHandler;
        }

        [HttpGet]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            return userHandler.LoginUser(loginRequest);
        }
    }
}
