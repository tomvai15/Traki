using Microsoft.EntityFrameworkCore;
using Traki.Api.Data;
using Traki.Api.Models;

namespace Traki.Api.Repositories
{
    public interface IUsersHandler
    {
        Task<User> GetUserByEmail(string email);
        Task<User> AddNewUser(User userToAdd);
    }

    public class UsersHandler : IUsersHandler
    {
        private readonly TrakiDbContext context;

        public UsersHandler(TrakiDbContext context)
        {
            this.context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            bool userExists = context.Users.Any(u => u.Email == email);

            if (!userExists) return null;

            return await context.Users.Where(u => u.Email == email).FirstAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            bool userExists = context.Users.Any(u => u.UserId == userId);

            if (!userExists) return null;

            return await context.Users.Where(u => u.UserId == userId).FirstAsync();
        }

        public async Task<User> AddNewUser(User userToAdd)
        {
            User user = await GetUserByEmail(userToAdd.Email);

            bool userExists = user != null;

            if (userExists) return null;

            user = context.Users.Add(userToAdd).Entity;

            await context.SaveChangesAsync();

            return user;
        }
    }
}
