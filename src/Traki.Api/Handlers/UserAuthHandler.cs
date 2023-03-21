using AutoMapper;
using System.Security.Claims;
using Traki.Api.Constants;
using Traki.Api.Contracts.Auth;
using Traki.Api.Cryptography;
using Traki.Api.Data.Repositories;
using Traki.Api.Exceptions;
using Traki.Api.Extensions;
using Traki.Api.Models;

namespace Traki.Api.Handlers
{
    public interface IUserAuthHandler
    {
        Task<User> GetUser(string email, string password);
        Task<IEnumerable<Claim>> CrateClaimsForUser(User user);
        Task<bool> TryCreateUser(CreateUserRequest createUserRequest);
    }
    public class UserAuthHandler : IUserAuthHandler
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        private readonly IHasherAdapter _hasherAdapter;

        public UserAuthHandler(IUsersRepository usersRepository, IMapper mapper, IHasherAdapter hasherAdapter)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
            _hasherAdapter = hasherAdapter;
        }
        public async Task<bool> TryCreateUser(CreateUserRequest createUserRequest)
        {
            User user = _mapper.Map<User>(createUserRequest);

            user.HashedPassword = _hasherAdapter.HashText(createUserRequest.Password);
            user.Role = Role.Manager;

            user = await _usersRepository.AddNewUser(user);

            bool isUserCreated = user != null;
            return isUserCreated;
        }

        public async Task<User> GetUser(string email, string password)
        {
            User user = await _usersRepository.GetUserByEmail(email);

            user.RequiresToBeNotNullEnity(new UnauthorizedException());

            bool isPasswordCorrect = _hasherAdapter.VerifyHashedText(password, user.HashedPassword);

            if (!isPasswordCorrect)
            {
                throw new UnauthorizedException();
            }

            return user;
        }

        public async Task<IEnumerable<Claim>> CrateClaimsForUser(User user)
        {
            return new[] {
               new Claim(Claims.UserId, user.UserId.ToString()),
               new Claim(ClaimTypes.Role, user.Role)
            };
        }
    }
}
