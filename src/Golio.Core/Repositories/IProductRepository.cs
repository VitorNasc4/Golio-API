using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Golio.Core.Entities;

namespace Golio.Core.Repositories
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product updatedProduct);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsAsync(string query = null);
    }
}
