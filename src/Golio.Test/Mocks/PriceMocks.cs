using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Golio.Core.Entities;

namespace Golio.Test.Mocks
{
    public class PriceMocks
    {
        public static Price GetValidPriceWithoutSuggestion(int? productId = null)
        {
            var store = StoreMocks.GetValidStore();
            var faker = new Faker<Price>("pt_BR")
                .CustomInstantiator(f => new Price()
                {
                    Store = store,
                    StoreId = store.Id,
                    Value = f.Finance.Random.Double(),
                    ProductId = productId ?? f.UniqueIndex,
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                    Suggestions = new List<Suggestion>()
                });

            var fakePrice = faker.Generate();
            return fakePrice;
        }
        public static Price GetValidPriceWithSuggestions(int? productId = null)
        {
            var id = new Random().Next(1, 101);
            var faker = new Faker<Price>("pt_BR")
                .CustomInstantiator(f => new Price()
                {
                    Id = id,
                    Store = StoreMocks.GetValidStore(),
                    Value = f.Finance.Random.Double(),
                    ProductId = productId ?? f.UniqueIndex,
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                    Suggestions = new List<Suggestion>()
                    {
                        GetValidSuggestion(id),
                        GetValidSuggestion(id)
                    }
                });

            var fakePrice = faker.Generate();
            return fakePrice;
        }
        public static Suggestion GetValidSuggestion(int? priceId = null)
        {
            var faker = new Faker<Suggestion>("pt_BR")
                .CustomInstantiator(f => new Suggestion()
                {
                    Value = f.Finance.Random.Double(),
                    AutorName = f.Name.FullName(),
                    AutorEmail = f.Internet.Email(),
                    PriceId = priceId ?? f.UniqueIndex,
                    CreatedAt = DateTime.UtcNow,
                    Active = true,
                }
                );

            var fakeSuggestion = faker.Generate();
            return fakeSuggestion;
        }

    }
}