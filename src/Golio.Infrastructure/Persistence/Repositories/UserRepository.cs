using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Golio.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GolioDbContext _dbContext;

        public UserRepository(GolioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            try
            {
                if (user.IsAdmin)
                {
                    user.SetUserAdmin();
                }
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao salvar usuário");
                Console.WriteLine(ex.Message);
            }
        }

        public Task<User> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
        {
            try
            {
                return _dbContext.Users
                    .SingleOrDefaultAsync(u => u.Email == email && u.Password == passwordHash);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar usuário por email e senha");
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return _dbContext.Users
                    .SingleOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar usuário por email");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _dbContext.Users
                    .SingleOrDefaultAsync(u => u.Id == id);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao buscar usuário por ID");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar salvar alterações de usuário");
                Console.WriteLine(ex.Message);
            }
        }
    }
}