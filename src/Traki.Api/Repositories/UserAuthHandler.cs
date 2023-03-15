using AutoMapper;
using Traki.Api.Constants;
using Traki.Api.Contracts;
using Traki.Api.Cryptography;
using Traki.Api.Models;

namespace Traki.Api.Repositories
{
    public interface IUserAuthHandler
    {
        Task<LoginResponse> LoginUser(LoginRequest loginRequest);
        Task<bool> TryCreateUser(CreateUserRequest createUserRequest);
    }
    public class UserAuthHandler : IUserAuthHandler
    {
        private readonly IJwtTokenGenerator jwtTokenGenerator;
        private readonly IUsersRepository usersHandler;
        private readonly IMapper mapper;
        private readonly IHasherAdapter hasherAdapter;

        public UserAuthHandler(IJwtTokenGenerator jwtTokenGenerator, IUsersRepository usersHandler, IMapper mapper, IHasherAdapter hasherAdapter)
        {
            this.jwtTokenGenerator = jwtTokenGenerator;
            this.usersHandler = usersHandler;
            this.mapper = mapper;
            this.hasherAdapter = hasherAdapter;
        }
        public async Task<bool> TryCreateUser(CreateUserRequest createUserRequest)
        {
            User user = mapper.Map<User>(createUserRequest);

            user.HashedPassword = hasherAdapter.HashText(createUserRequest.Password);
            user.Role = Role.Manager;

            user = await usersHandler.AddNewUser(user);

            bool isUserCreated = user != null;
            return isUserCreated;
        }

        public async Task<LoginResponse> LoginUser(LoginRequest loginRequest)
        {
            User user = await usersHandler.GetUserByEmail(loginRequest.Email);

            if (user == null) return null;

            bool isPasswordCorrect = hasherAdapter.VerifyHashedText(loginRequest.Password, user.HashedPassword);

            if (!isPasswordCorrect) return null;

            string token = jwtTokenGenerator.GenerateToken(user);

            return new LoginResponse
            {
                Email = user.Email,
                Token = token
            };
        }
    }
}
