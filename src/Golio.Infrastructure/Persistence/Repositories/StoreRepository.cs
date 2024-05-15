using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Golio.Infrastructure.Persistence.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly GolioDbContext _dbContext;

        public StoreRepository(GolioDbContext dbContext)
        {
            _dbContext = dbContext; ;
        }

        public async Task AddStoreAsync(Store store)
        {
            try
            {
                await _dbContext.Stores
                    .AddAsync(store);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao registrar loja");
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<Store> GetStoreByIdAsync(int storeId)
        {
            try
            {
                var store = await _dbContext.Stores
                    .SingleOrDefaultAsync(s => s.Id == storeId);

                return store;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar loja pelo ID");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Store> GetStoreByNameAsync(string storeName)
        {
            try
            {
                var store = await _dbContext.Stores
                    .SingleOrDefaultAsync(s => s.Name == storeName);

                return store;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar loja pelo ID");
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Store>> GetStoresByQueryAsync(string query)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                {
                    var storesWithQuery = await _dbContext.Stores
                        .Where(store => store.Name.Contains(query))
                        .ToListAsync();
                }

                var stores = await _dbContext.Stores
                    .ToListAsync();

                return stores;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao consultar todas as lojas");
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
                Console.WriteLine("Erro ao salvar loja");
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateStoreAsync(Store updatedStore)
        {
            var store = await _dbContext.Stores
                .SingleOrDefaultAsync(s => s.Id == updatedStore.Id);

            if (store == null)
            {
                Console.WriteLine("Loja não encontrada");
                return;
            }

            store.Name = updatedStore.Name;
            store.Address = updatedStore.Address;
            store.City = updatedStore.City;
            store.State = updatedStore.State;
            store.ZipCode = updatedStore.ZipCode;

            await _dbContext.SaveChangesAsync();
        }
    }
}