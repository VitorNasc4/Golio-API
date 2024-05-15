using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Golio.Core.Entities;

namespace Golio.Core.Repositories
{
    public interface IStoreRepository
    {
        Task AddStoreAsync(Store store);
        Task UpdateStoreAsync(Store updatedStore);
        Task<Store> GetStoreByIdAsync(int storeId);
        Task<Store> GetStoreByNameAsync(string storeName);
        Task<List<Store>> GetStoresByQueryAsync(string query);
        Task SaveChangesAsync();
    }
}
