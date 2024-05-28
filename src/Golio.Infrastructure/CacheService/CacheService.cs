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
                    Console.WriteLine($"Key {key} do not exist in Cache");
                    return default;
                }

                Console.WriteLine($"Key {key} from Cache");
                return JsonSerializer.Deserialize<T>(objectString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving value from Cache: {ex.Message}");
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
                Console.WriteLine($"Setting key {key} in Cache");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting value in Cache: {ex.Message}");
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
                Console.WriteLine($"Setting key {defaultQuery} in Cache");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting value in Cache: {ex.Message}");
            }
        }
    }
}