using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> AddNewUser(User userToAdd);
    }
}
