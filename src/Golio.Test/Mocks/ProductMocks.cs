using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Golio.Core.Entities;

namespace Golio.Test.Mocks
{
    public class ProductMocks
    {
        public static Product GetValidCompleteProduct(int? productId = null)
        {
            var id = productId ?? new Random().Next(1, 101);
            var price = PriceMocks.GetValidPriceWithSuggestions(id);
            var faker = new Faker<Product>("pt_BR")
                .CustomInstantiator(f => new Product()
                {
                    Id = id,
                    Name = f.Commerce.Product(),
                    Brand = f.Commerce.Department(),
                    Volume = f.Finance.Random.Int(),
                    Prices = new List<Price>() { price },
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                });

            var fakeProduct = faker.Generate();
            return fakeProduct;
        }

        public static Product GetValidProductWithoutPriceSuggestion(int? productId = null)
        {
            var id = productId ?? new Random().Next(1, 101);
            var price = PriceMocks.GetValidPriceWithoutSuggestion(id);
            var faker = new Faker<Product>("pt_BR")
                .CustomInstantiator(f => new Product()
                {
                    Id = id,
                    Name = f.Commerce.Product(),
                    Brand = f.Commerce.Department(),
                    Volume = f.Finance.Random.Int(),
                    Prices = new List<Price>() { price },
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                });

            var fakeProduct = faker.Generate();
            return fakeProduct;
        }

        public static Product GetValidProductWithoutPrice(int? productId = null)
        {
            var id = productId ?? new Random().Next(1, 101);
            var faker = new Faker<Product>("pt_BR")
                .CustomInstantiator(f => new Product()
                {
                    Id = id,
                    Name = f.Commerce.Product(),
                    Brand = f.Commerce.Department(),
                    Volume = f.Finance.Random.Int(),
                    Prices = new List<Price>(),
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                });

            var fakeProduct = faker.Generate();
            return fakeProduct;
        }

    }
}