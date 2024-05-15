using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Application.InputModels;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class CreateProductCommandHandlerTest
    {
        [Fact]
        public async Task InputDataIsOkWithPrice_Executed_ReturnProductId()
        {
            var storeInputModel = new CreateStoreInputModel()
            {
                Name = "Loja",
                Address = "Endereço",
                City = "Cidade",
                State = "Estado",
                ZipCode = "CEP"
            };

            var priceInputModel = new CreatePriceInputModel()
            {
                Value = 10,
                Store = storeInputModel
            };

            var priceInputModelList = new List<CreatePriceInputModel>() { priceInputModel };

            var productRepositoryMock = new Mock<IProductRepository>();

            var createProductCommand = new CreateProductCommand
            {
                Name = "DummyName",
                Brand = "DummyBrand",
                Volume = 100,
                Prices = priceInputModelList
            };

            var cacheServiceMock = new Mock<ICacheService>();

            var sut = new CreateProductCommandHandler(productRepositoryMock.Object, cacheServiceMock.Object);
            var id = await sut.Handle(createProductCommand, new CancellationToken());

            Assert.True(id >= 0);
            productRepositoryMock.Verify(p => p.AddProductAsync(It.Is<Product>(
                p => p.Name == createProductCommand.Name &&
                p.Brand == createProductCommand.Brand &&
                p.Volume == createProductCommand.Volume &&
                p.Prices.FirstOrDefault()!.Value == priceInputModel.Value
                )), Times.Once);
        }
        [Fact]
        public async Task InputDataIsOkWithoutPrice_Executed_ReturnProductId()
        {
            var productRepositoryMock = new Mock<IProductRepository>();

            var createProductCommand = new CreateProductCommand
            {
                Name = "DummyName",
                Brand = "DummyBrand",
                Volume = 100,
                Prices = new List<CreatePriceInputModel>()
            };

            var cacheServiceMock = new Mock<ICacheService>();

            var sut = new CreateProductCommandHandler(productRepositoryMock.Object, cacheServiceMock.Object);
            var id = await sut.Handle(createProductCommand, new CancellationToken());

            Assert.True(id >= 0);
            productRepositoryMock.Verify(p => p.AddProductAsync(It.Is<Product>(
                p => p.Name == createProductCommand.Name &&
                p.Brand == createProductCommand.Brand &&
                p.Volume == createProductCommand.Volume
                )), Times.Once);
        }
    }
}