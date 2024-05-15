using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Golio.Core.Entities;

namespace Golio.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash);
        Task<User> GetUserByEmailAsync(string email);
        Task AddAsync(User user);
    }
}
