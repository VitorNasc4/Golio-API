using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class updateStoreCommandHandlerTest
    {
        [Fact]
        public async Task UpdateStoreCommandHandlerTest_InputDataIsOk_Executed()
        {
            var storeRepositoryMock = new Mock<IStoreRepository>();

            var updateStoreCommand = new UpdateStoreCommand
            {
                StoreId = 1,
                Name = "Loja",
                Address = "Edereço",
                City = "Cidade",
                State = "Estado",
                ZipCode = "CEP"
            };

            var cacheServiceMock = new Mock<ICacheService>();

            var sut = new UpdateStoreCommandHandler(storeRepositoryMock.Object, cacheServiceMock.Object);
            var id = await sut.Handle(updateStoreCommand, new CancellationToken());

            storeRepositoryMock.Verify(p => p.UpdateStoreAsync(It.Is<Store>(store =>
                store.Name == updateStoreCommand.Name &&
                store.Address == updateStoreCommand.Address &&
                store.City == updateStoreCommand.City &&
                store.State == updateStoreCommand.State &&
                store.ZipCode == updateStoreCommand.ZipCode &&
                store.Id == updateStoreCommand.StoreId
                )), Times.Once);
        }
    }
}