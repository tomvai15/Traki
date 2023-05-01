using AutoMapper;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Traki.Infrastructure.Repositories
{
    public class UserRepository : IUsersRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null) return null;

            return _mapper.Map<User>(user);
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null) return null;

            return _mapper.Map<User>(user);
        }

        public async Task<User> AddNewUser(User userToAdd)
        {
            User user = await GetUserByEmail(userToAdd.Email);

            if (user != null) return null;

            var userEntity = _mapper.Map<UserEntity>(userToAdd);

            userEntity = _context.Users.Add(userEntity).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<User>(userEntity);
        }

        public async Task<User> UpdateUser(User user)
        {
            var userEntity = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            userEntity.Status = user.Status;
            userEntity.EncryptedRefreshToken = user.EncryptedRefreshToken;
            userEntity.HashedPassword = user.HashedPassword;
            userEntity.RegisterId = user.RegisterId;
            userEntity.DeviceToken = user.DeviceToken;
            userEntity.UserIconBase64 = user.UserIconBase64;
            userEntity.RefreshToken = user.RefreshToken;
            userEntity.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;

            await _context.SaveChangesAsync();
            return _mapper.Map<User>(userEntity);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<User>>(users);
        }

        public async Task<IEnumerable<User>> GetUsersByQuery(Func<User, bool> filter)
        {
            Func<UserEntity, bool> func = (x) => {
                var p = _mapper.Map<User>(x);
                return filter(p);
            };
            var users = _context.Users.Where(func).ToList();

            return _mapper.Map<IEnumerable<User>>(users);
        }
    }
}
