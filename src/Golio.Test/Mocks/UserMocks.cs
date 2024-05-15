using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Golio.Core.Entities;

namespace Golio.Test.Mocks
{
    public class UserMocks
    {
        public static User GetValidClientUser()
        {
            var faker = new Faker<User>("pt_BR")
                .CustomInstantiator(f => new User(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Internet.Password(),
                    false
                ));

            var fakeUser = faker.Generate();
            return fakeUser;
        }
        public static User GetValidAdmin()
        {
            var faker = new Faker<User>("pt_BR")
                .CustomInstantiator(f => new User(
                    f.Person.FullName,
                    f.Internet.Email(),
                    f.Internet.Password(),
                    true
                ));

            var fakeUser = faker.Generate();
            return fakeUser;
        }

    }
}