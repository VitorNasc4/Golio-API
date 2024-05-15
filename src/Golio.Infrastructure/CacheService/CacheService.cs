using System;
using System.Text.Json;
using System.Threading.Tasks;
using Golio.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace Golio.Infrastructure.CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IProductRepository _productRepository;
        private readonly IConfiguration _configuration;

        public CacheService(IDistributedCache cache, IProductRepository productRepository, IConfiguration configuration)
        {
            _cache = cache;
            _productRepository = productRepository;
            _configuration = configuration;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var objectString = await _cache.GetStringAsync(key);

                if (string.IsNullOrWhiteSpace(objectString))
                {
                    Console.WriteLine($"Key {key} não existe em Cache");
                    return default;
                }

                Console.WriteLine($"Key {key} buscada em Cache");
                return JsonSerializer.Deserialize<T>(objectString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter valor em Cache: {ex.Message}");
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T data)
        {
            try
            {
                var memoryCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                    SlidingExpiration = TimeSpan.FromHours(4)
                };

                var objectString = JsonSerializer.Serialize(data);
                await _cache.SetStringAsync(key, objectString, memoryCacheEntryOptions);
                Console.WriteLine($"Registrando key {key} em Cache");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao setar valor em Cache: {ex.Message}");
            }
        }

        public async Task UpdateDefaultProductQueryAsync()
        {
            try
            {
                var defaultQuery = _configuration["Configs:DefaultProductQuery"];
                var products = await _productRepository.GetProductsAsync();

                await _cache.RemoveAsync(defaultQuery);

                var memoryCacheEntryOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                    SlidingExpiration = TimeSpan.FromHours(4)
                };

                var objectString = JsonSerializer.Serialize(products);
                await _cache.SetStringAsync(defaultQuery, objectString, memoryCacheEntryOptions);
                Console.WriteLine($"Registrando key {defaultQuery} em Cache");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao setar valor em Cache: {ex.Message}");
            }
        }
    }
}