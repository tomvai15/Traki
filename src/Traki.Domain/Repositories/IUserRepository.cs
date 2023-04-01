﻿using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUserByEmail(string email);
        Task<User> AddNewUser(User userToAdd);
        Task UpdateUser(User user);
    }
}
