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
                    Console.WriteLine("Produto não encontrado");
                    return;
                }

                var priceAlreadyExist = product.Prices.Any(p => p.StoreId == price.StoreId);
                if (priceAlreadyExist)
                {
                    Console.WriteLine("Preço já registrado");
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
                Console.WriteLine("Erro ao registrar preço");
                Console.WriteLine(ex.Message);
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
                Console.WriteLine("Erro ao consultar preços pelo ID");
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
                Console.WriteLine("Erro ao consultar salvar alterações de preço");
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdatePriceAsync(Price updatedPrice)
        {
            var price = await _dbContext.Prices
                .SingleOrDefaultAsync(p => p.Id == updatedPrice.Id);

            if (price == null)
            {
                Console.WriteLine("Price não encontrada");
                return;
            }

            price.Value = updatedPrice.Value;

            await _dbContext.SaveChangesAsync();
        }
    }
}