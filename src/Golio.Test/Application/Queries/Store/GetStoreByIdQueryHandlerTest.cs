using Golio.Application.Queries.GetProductById;
using Golio.Application.Queries.GetProducts;
using Golio.Core.Repositories;
using Golio.Test.Mocks;
using Moq;
using Xunit;

namespace Golio.Test.Application.Queries
{
    public class GetStoreByIdQueryHandlerTest
    {
        [Fact]
        public async void GetStoreByIdQueryHandlerTest_OnSuccess()
        {
            var store = StoreMocks.GetValidStore();

            var storeRepositoryMock = new Mock<IStoreRepository>();
            storeRepositoryMock
                .Setup(ur => ur.GetStoreByIdAsync(store.Id))
                .ReturnsAsync(store);

            var getStoreByIdQuery = new GetStoreByIdQuery(store.Id);
            var sut = new GetStoreByIdQueryHandler(storeRepositoryMock.Object);
            var result = await sut.Handle(getStoreByIdQuery, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(store.Name, result.Name);
            Assert.Equal(store.Address, result.Address);
            Assert.Equal(store.City, result.City);
            Assert.Equal(store.State, result.State);
            Assert.Equal(store.ZipCode, result.ZipCode);

            storeRepositoryMock.Verify(ur => ur.GetStoreByIdAsync(store.Id), Times.Once);
        }
    }
}