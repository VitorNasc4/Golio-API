using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Golio.Infrastructure.Persistence.Repositories
{
    public class PriceRepository : IPriceRepository
    {
        private readonly GolioDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly IStoreRepository _storeRepository;

        public PriceRepository(GolioDbContext dbContext, IProductRepository productRepository, IStoreRepository storeRepository)
        {
            _dbContext = dbContext;
            _productRepository = productRepository;
            _storeRepository = storeRepository;
        }
        public async Task AddPriceAsync(Price price)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(price.ProductId);
                if (product == null)
                {
                    Console.WriteLine($"Product with ID {price.ProductId} not found");
                    return;
                }

                var priceAlreadyExist = product.Prices.Any(p => p.StoreId == price.StoreId);
                if (priceAlreadyExist)
                {
                    Console.WriteLine($"Price whit StoreId {price.StoreId} already exists");
                    return;
                }

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

                product.Prices.Add(price);

                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering price: {ex.Message}");
            }
        }

        public async Task<Price> GetPriceByIdAsync(int priceId)
        {
            try
            {
                var price = await _dbContext.Prices
                    .Include(p => p.Store)
                    .Include(p => p.Suggestions)
                    .SingleOrDefaultAsync(p => p.Id == priceId);

                return price;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error on searching prices by Id {priceId}: {ex.Message}");
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
                Console.WriteLine($"Error saving price changes: {ex.Message}");
            }
        }

        public async Task UpdatePriceAsync(Price updatedPrice)
        {
            var price = await _dbContext.Prices
                .SingleOrDefaultAsync(p => p.Id == updatedPrice.Id);

            if (price == null)
            {
                Console.WriteLine($"Price with ID {updatedPrice.Id} not found");
                return;
            }

            foreach (var suggestion in price.Suggestions)
            {
                _dbContext.Suggestions.Remove(suggestion);
            }

            price.Suggestions = new List<Suggestion>();
            price.Value = updatedPrice.Value;

            await _dbContext.SaveChangesAsync();
        }
    }
}