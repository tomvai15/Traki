using AutoMapper;
using Traki.Infrastructure.Entities;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Traki.Infrastructure.Repositories
{
    public class UsersHandler : IUsersRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public UsersHandler(TrakiDbContext context, IMapper mapper)
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

            var userEntity = _mapper.Map<UserEntity>(user);

            userEntity = _context.Users.Add(userEntity).Entity;

            await _context.SaveChangesAsync();

            return _mapper.Map<User>(userEntity);
        }

        public async Task UpdateUser(User user)
        {
            var userEntity = _context.Users.FirstOrDefault(u => u.Id == user.Id);

            userEntity.Status = user.Status;
            userEntity.EncryptedRefreshToken = user.EncryptedRefreshToken;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<User>>(users);
        }
    }
}
