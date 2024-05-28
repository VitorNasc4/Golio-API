using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Golio.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly GolioDbContext _dbContext;
        private readonly IStoreRepository _storeRepository;


        public ProductRepository(GolioDbContext dbContext, IStoreRepository storeRepository)
        {
            _dbContext = dbContext;
            _storeRepository = storeRepository;
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                foreach (var price in product.Prices)
                {
                    var store = price.Store;
                    var existingStore = await _storeRepository.GetStoreByNameAsync(store.Name);
                    if (existingStore == null)
                    {
                        await _storeRepository.AddStoreAsync(store);
                    }
                    else
                    {
                        price.Store = existingStore;
                    }
                }
                await _dbContext.Products.AddAsync(product);

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _dbContext.Products
                    .Include(p => p.Prices)
                        .ThenInclude(price => price.Store)
                    .Include(p => p.Prices)
                        .ThenInclude(price => price.Suggestions)
                    .SingleOrDefaultAsync(p => p.Id == id);

                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when searching for product by ID {id}: {ex.Message}");
                return null;
            }
        }
        public async Task<List<Product>> GetProductsAsync(string query)
        {
            try
            {
                if (!string.IsNullOrEmpty(query))
                {
                    var productsFromQuery = await _dbContext.Products
                            .Where(p => p.Name.Contains(query))
                            .Include(p => p.Prices)
                                .ThenInclude(price => price.Store)
                            .Include(p => p.Prices)
                                .ThenInclude(price => price.Suggestions)
                            .ToListAsync();


                    return productsFromQuery;
                }

                var products = await _dbContext.Products
                    .Include(p => p.Prices)
                        .ThenInclude(price => price.Suggestions)
                    .Include(p => p.Prices)
                        .ThenInclude(price => price.Store)
                    .ToListAsync();


                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when searching for products by query: {ex.Message}");
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
                Console.WriteLine("Error saving product");
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateProductAsync(Product updatedProduct)
        {
            var product = await _dbContext.Products
                .Include(p => p.Prices)
                .SingleOrDefaultAsync(p => p.Id == updatedProduct.Id);

            if (product == null)
            {
                Console.WriteLine($"Product with ID {updatedProduct.Id} not found");
                return;
            }

            product.Name = updatedProduct.Name;
            product.Brand = updatedProduct.Brand;
            product.Volume = updatedProduct.Volume;

            await _dbContext.SaveChangesAsync();
        }
    }
}