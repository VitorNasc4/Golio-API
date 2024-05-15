using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Golio.Core.Entities;

namespace Golio.Test.Mocks
{
    public class StoreMocks
    {
        public static Store GetValidStore()
        {
            var faker = new Faker<Store>("pt_BR")
                .CustomInstantiator(f => new Store()
                {
                    Id = f.UniqueIndex,
                    Name = f.Company.CompanyName(),
                    Address = f.Address.StreetName(),
                    City = f.Address.City(),
                    State = f.Address.State(),
                    ZipCode = f.Address.ZipCode(),
                    CreatedAt = DateTime.UtcNow,
                    Active = true
                });

            var fakeStore = faker.Generate();
            return fakeStore;
        }

    }
}