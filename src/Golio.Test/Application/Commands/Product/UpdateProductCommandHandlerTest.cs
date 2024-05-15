using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class UpdateProductCommandHandlerTest
    {
        [Fact]
        public async Task UpdateProductCommandHandlerTest_InputDataIsOk_Executed()
        {
            var productRepositoryMock = new Mock<IProductRepository>();

            var updateProductCommand = new UpdateProductCommand
            {
                ProductId = 1,
                Name = "Nome",
                Brand = "Marca",
                Volume = 100,
            };

            var cacheServiceMock = new Mock<ICacheService>();

            var sut = new UpdateProductCommandHandler(productRepositoryMock.Object, cacheServiceMock.Object);
            var id = await sut.Handle(updateProductCommand, new CancellationToken());

            productRepositoryMock.Verify(p => p.UpdateProductAsync(It.Is<Product>(product =>
                product.Name == updateProductCommand.Name &&
                product.Brand == updateProductCommand.Brand &&
                product.Volume == updateProductCommand.Volume &&
                product.Id == updateProductCommand.ProductId
                )), Times.Once);
        }
    }
}