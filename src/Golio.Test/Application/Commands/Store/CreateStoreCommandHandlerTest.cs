using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class CreateStoreCommandHandlerTest
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnStoreId()
        {
            var storeRepositoryMock = new Mock<IStoreRepository>();

            var createStoreCommand = new CreateStoreCommand
            {
                Name = "Loja",
                Address = "Endereço",
                City = "Cidade",
                State = "State",
                ZipCode = "CEP"
            };

            var sut = new CreateStoreCommandHandler(storeRepositoryMock.Object);
            var id = await sut.Handle(createStoreCommand, new CancellationToken());

            Assert.True(id >= 0);
            storeRepositoryMock.Verify(p => p.AddStoreAsync(It.Is<Store>(store =>
                store.Name == createStoreCommand.Name &&
                store.Address == createStoreCommand.Address &&
                store.City == createStoreCommand.City &&
                store.State == createStoreCommand.State &&
                store.ZipCode == createStoreCommand.ZipCode
                )), Times.Once);
        }
    }
}