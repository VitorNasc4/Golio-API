using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class CreatePriceCommandHandlerTest
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnPriceId()
        {
            var priceRepositoryMock = new Mock<IPriceRepository>();
            var storeRepositoryMock = new Mock<IStoreRepository>();


            var createPriceCommand = new CreatePriceCommand
            {
                Value = 100,
                ProductId = 1,
                StoreId = 1,
            };

            var cacheServiceMock = new Mock<ICacheService>();

            var sut = new CreatePriceCommandHandler(priceRepositoryMock.Object, storeRepositoryMock.Object, cacheServiceMock.Object);
            var id = await sut.Handle(createPriceCommand, new CancellationToken());

            Assert.True(id >= 0);
            priceRepositoryMock.Verify(p => p.AddPriceAsync(It.Is<Price>(price => price.Value == createPriceCommand.Value)), Times.Once);
            storeRepositoryMock.Verify(s => s.GetStoreByIdAsync(createPriceCommand.StoreId), Times.Once);
        }
    }
}