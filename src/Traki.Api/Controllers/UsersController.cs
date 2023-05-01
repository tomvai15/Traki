using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Traki.Api.Contracts.Auth;
using Traki.Api.Contracts.User;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = Role.Administrator)]
    public class UsersController : ControllerBase
    {
        private readonly IUserAuthHandler _userAuthHandler;
        private readonly IUsersRepository _usersRepository;
        private readonly IUserHandler _userHandler;
        private readonly IMapper _mapper;

        public UsersController(IUserHandler userHandler, IUserAuthHandler userAuthHandler, IUsersRepository usersRepository, IMapper mapper)
        {
            _userHandler = userHandler;
            _userAuthHandler = userAuthHandler;
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        [HttpGet("pending/{registerId}")]
        public async Task<ActionResult<GetUserResponse>> GetUserByRegisterId(string registerId)
        {
            var users = await _usersRepository.GetUsersByQuery(x => x.RegisterId == registerId);
            var response = new GetUserResponse
            {
                User = _mapper.Map<UserDto>(users.First())
            };
            return Ok(response);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<GetUserResponse>> GetUser(int userId)
        {
            var user = await _usersRepository.GetUserById(userId);

            var response = new GetUserResponse
            {
                User = _mapper.Map<UserDto>(user)
            };
            return Ok(response);
        }

        [HttpPost("{userId}/status")]
        public async Task<ActionResult> UpdateStatus(int userId, [FromBody] UpdateUserStatusRequest updateUserStatusRequest)
        {
            var user = await _usersRepository.GetUserById(userId);

            if (updateUserStatusRequest.Status == UserStatus.Active || updateUserStatusRequest.Status == UserStatus.Blocked)
            {
                user.Status = updateUserStatusRequest.Status;
                await _usersRepository.UpdateUser(user);
            }
            else
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserRequest createUserRequest)
        {
            var user = _mapper.Map<User>(createUserRequest.User);

            await _userHandler.CreateUser(user);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<GetUsersResponse>> GetUsers()
        {
            var users = await _usersRepository.GetUsers();

            var response = new GetUsersResponse
            {
                Users = _mapper.Map<IEnumerable<UserDto>>(users)
            };
            return Ok(response);
        }


        /*
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserRequest createUserRequest)
        {
            bool isUserAdded = await _userAuthHandler.TryCreateUser(createUserRequest.Email, createUserRequest.Password);

            if (!isUserAdded)
            {
                return BadRequest(new { message = "Email is already taken" });
            }
            return NoContent();
        }*/
    }
}
