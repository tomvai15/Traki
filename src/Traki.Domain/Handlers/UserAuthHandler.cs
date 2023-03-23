using AutoMapper;
using System.Security.Claims;
using Traki.Domain.Constants;
using Traki.Domain.Cryptography;
using Traki.Domain.Exceptions;
using Traki.Domain.Extensions;
using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Domain.Handlers
{
    public interface IUserAuthHandler
    {
        Task<User> GetUser(string email, string password);
        Task<IEnumerable<Claim>> CrateClaimsForUser(User user);
        Task<bool> TryCreateUser(string email, string password);
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

        public async Task<bool> TryCreateUser(string email, string password)
        {
            User user = new User { Email = email };

            user.HashedPassword = _hasherAdapter.HashText(password);
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
