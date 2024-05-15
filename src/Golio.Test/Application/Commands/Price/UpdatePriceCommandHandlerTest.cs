using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class UpdatePriceCommandHandlerTest
    {
        [Fact]
        public async Task UpdatePriceCommandHandlerTest_InputDataIsOk_Executed()
        {
            var priceRepositoryMock = new Mock<IPriceRepository>();

            var updatePriceCommand = new UpdatePriceCommand
            {
                Value = 100,
                PriceId = 1,
            };

            var cacheServiceMock = new Mock<ICacheService>();

            var sut = new UpdatePriceCommandHandler(priceRepositoryMock.Object, cacheServiceMock.Object);
            var id = await sut.Handle(updatePriceCommand, new CancellationToken());

            priceRepositoryMock.Verify(p => p.UpdatePriceAsync(It.Is<Price>(price =>
                price.Value == updatePriceCommand.Value &&
                price.Id == updatePriceCommand.PriceId
                )), Times.Once);
        }
    }
}