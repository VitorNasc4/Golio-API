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
                Console.WriteLine($"Error adding product: {ex.Message}");
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
                Console.WriteLine($"Error searching store by ID {storeId}: {ex.Message}");
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
                Console.WriteLine($"Error searching store by name {storeName}: {ex.Message}");
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
                Console.WriteLine($"Error searching all stores: {ex.Message}");
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
                Console.WriteLine($"Error saving stores: {ex.Message}");
            }
        }

        public async Task UpdateStoreAsync(Store updatedStore)
        {
            var store = await _dbContext.Stores
                .SingleOrDefaultAsync(s => s.Id == updatedStore.Id);

            if (store == null)
            {
                Console.WriteLine($"Store with ID {updatedStore.Id} nor found");
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