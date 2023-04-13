using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int userId);
        Task<User> GetUserByEmail(string email);
        Task<User> AddNewUser(User userToAdd);
        Task UpdateUser(User user);
        Task<IEnumerable<User>> GetUsersByQuery(Func<User, bool> filter);
    }
}
