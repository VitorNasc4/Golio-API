using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.Queries.GetProductById;
using Golio.Application.Queries.GetProducts;
using Golio.Application.Queries.GetUser;
using Golio.Core.Repositories;
using Golio.Test.Mocks;
using Moq;
using Xunit;

namespace Golio.Test.Application.Queries
{
    public class GetProductByIdQueryHandlerTest
    {
        [Fact]
        public async void GetProductByIdQueryHandlerTest_WithPriceSuggestions_OnSuccess()
        {
            var product = ProductMocks.GetValidCompleteProduct();

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock
                .Setup(p => p.GetProductByIdAsync(product.Id))
                .ReturnsAsync(product);

            var getProductByIdQuery = new GetProductByIdQuery(product.Id);
            var sut = new GetProductByIdQueryHandler(productRepositoryMock.Object);
            var result = await sut.Handle(getProductByIdQuery, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Brand, result.Brand);
            Assert.Equal(product.Volume, result.Volume);

            foreach (var price in product.Prices)
            {
                var priceResult = result.Prices.FirstOrDefault(s => s.StoreName == price.Store.Name);
                Assert.Equal(price.Value, priceResult!.Value);


                foreach (var suggestion in price.Suggestions)
                {
                    var suggestionResult = priceResult.Suggestions.FirstOrDefault(s => s.Autor == suggestion.AutorName);
                    Assert.Equal(suggestion.Value, suggestionResult!.Value);
                    Assert.Equal(suggestion.AutorName, suggestionResult!.Autor);
                }
            }


            productRepositoryMock.Verify(p => p.GetProductByIdAsync(product.Id), Times.Once);
        }

        [Fact]
        public async void GetProductByIdQueryHandlerTest_WithoutPriceSuggestions_OnSuccess()
        {
            var product = ProductMocks.GetValidProductWithoutPriceSuggestion();

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock
                .Setup(p => p.GetProductByIdAsync(product.Id))
                .ReturnsAsync(product);

            var getProductByIdQuery = new GetProductByIdQuery(product.Id);
            var sut = new GetProductByIdQueryHandler(productRepositoryMock.Object);
            var result = await sut.Handle(getProductByIdQuery, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Brand, result.Brand);
            Assert.Equal(product.Volume, result.Volume);

            foreach (var price in product.Prices)
            {
                var priceResult = result.Prices.FirstOrDefault(s => s.StoreName == price.Store.Name);
                Assert.Equal(price.Value, priceResult!.Value);
                Assert.Empty(priceResult.Suggestions);
            }


            productRepositoryMock.Verify(p => p.GetProductByIdAsync(product.Id), Times.Once);
        }


        [Fact]
        public async void GetProductByIdQueryHandlerTest_WithoutPrice_OnSuccess()
        {
            var product = ProductMocks.GetValidProductWithoutPrice();

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock
                .Setup(p => p.GetProductByIdAsync(product.Id))
                .ReturnsAsync(product);

            var getProductByIdQuery = new GetProductByIdQuery(product.Id);
            var sut = new GetProductByIdQueryHandler(productRepositoryMock.Object);
            var result = await sut.Handle(getProductByIdQuery, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Brand, result.Brand);
            Assert.Equal(product.Volume, result.Volume);
            Assert.Empty(result.Prices);


            productRepositoryMock.Verify(p => p.GetProductByIdAsync(product.Id), Times.Once);
        }
    }
}