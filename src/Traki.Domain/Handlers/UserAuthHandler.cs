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
        Task<IEnumerable<Claim>> CreateClaimsForUser(User user);
        Task ActivateAccount(string registerId, string code, string password);
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

        public async Task ActivateAccount(string registerId, string code, string password)
        {
            var users = await _usersRepository.GetUsersByQuery(x => x.RegisterId == registerId);
            var user = users.First();

            user.RequiresToBeNotNullEnity();

            bool correctCode = _hasherAdapter.VerifyHashedText(code, user.HashedPassword);

            if (!correctCode) 
            { 
                throw new UnauthorizedException();
            }

            string hashedPassword = _hasherAdapter.HashText(password);

            user.HashedPassword = hashedPassword;
            user.RegisterId = null;

            await _usersRepository.UpdateUser(user);
        }

        public async Task<bool> TryCreateUser(string email, string password)
        {
            User user = new User { Email = email };

            user.HashedPassword = _hasherAdapter.HashText(password);
            user.Role = Role.ProjectManager;

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

            if (!user.Status.Equals(UserStatus.Active))
            {
                throw new UnauthorizedException();
            }

            return user;
        }

        public async Task<IEnumerable<Claim>> CreateClaimsForUser(User user)
        {
            return new[] {
               new Claim(Claims.UserId, user.Id.ToString()),
               new Claim(ClaimTypes.Role, user.Role)
            };
        }
    }
}
