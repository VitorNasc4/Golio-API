using Golio.Application.Queries.GetProductById;
using Golio.Application.Queries.GetProducts;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Test.Mocks;
using Moq;
using Xunit;

namespace Golio.Test.Application.Queries
{
    public class GetStoresByQueryHandlerTest
    {
        [Fact]
        public async void GetStoresByQueryHandlerTest_OnSuccess()
        {
            var store1 = StoreMocks.GetValidStore();
            var store2 = StoreMocks.GetValidStore();
            store2.Name = store1.Name + " Diferente";

            var storeList = new List<Store>()
            {
                store1,
                store2
            };
            var query = "DummyQuery";

            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock
                .Setup(ur => ur.GetStoresByQueryAsync(query))
                .ReturnsAsync(storeList);

            var getStoresByQuery = new GetStoresByQuery(query);
            var sut = new GetStoresByQueryHandler(storeRepositoryMock.Object);
            var result = await sut.Handle(getStoresByQuery, new CancellationToken());

            Assert.NotNull(result);
            foreach (var store in storeList)
            {
                var storeFromResult = result.FirstOrDefault(s => s.Name == store.Name);
                Assert.Equal(store.Name, storeFromResult!.Name);
                Assert.Equal(store.Address, storeFromResult!.Address);
                Assert.Equal(store.City, storeFromResult!.City);
                Assert.Equal(store.State, storeFromResult!.State);
                Assert.Equal(store.ZipCode, storeFromResult!.ZipCode);
            }

            storeRepositoryMock.Verify(ur => ur.GetStoresByQueryAsync(query), Times.Once);
        }
    }
}