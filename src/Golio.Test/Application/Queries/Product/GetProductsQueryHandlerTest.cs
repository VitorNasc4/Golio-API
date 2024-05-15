using Golio.Application.Queries.GetProducts;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Golio.Test.Mocks;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Golio.Test.Application.Queries
{
    public class GetProductsQueryHandlerTest
    {
        public List<Product> DifferentiatingProducts(List<Product> productList)
        {
            var productIndex = 1;
            var suggestionIndex = 1;
            var priceIndex = 1;

            foreach (var product in productList)
            {
                product.Name += productIndex.ToString();
                productIndex++;

                if (product.Prices.Count > 0)
                {
                    foreach (var price in product.Prices)
                    {
                        price.Store.Name += priceIndex.ToString();
                        priceIndex++;

                        if (price.Suggestions.Count > 0)
                        {
                            foreach (var suggestion in price.Suggestions)
                            {
                                suggestion.AutorName += suggestionIndex.ToString();
                                suggestionIndex++;
                            }
                        }
                    }
                }
            }

            return productList;
        }

        [Fact]
        public async void GetProductsQueryHandlerTest_WithPriceSuggestions_NotUsingQuery_FromBase_OnSuccess()
        {
            var productList = new List<Product>()
            {
                ProductMocks.GetValidCompleteProduct(),
                ProductMocks.GetValidCompleteProduct()
            };

            DifferentiatingProducts(productList);

            var query = "";

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock
                .Setup(p => p.GetProductsAsync(query))
                .ReturnsAsync(productList);

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock
                .Setup(cs => cs.GetAsync<List<Product>>(It.IsAny<string>()))
                .ReturnsAsync((List<Product>)null!);
            var configurationMock = new Mock<IConfiguration>();

            var getProductsQuery = new GetProductsQuery(query);
            var sut = new GetProductsQueryHandler(productRepositoryMock.Object, cacheServiceMock.Object, configurationMock.Object);
            var result = await sut.Handle(getProductsQuery, new CancellationToken());

            Assert.NotNull(result);
            foreach (var product in productList)
            {
                var productFromResult = result.FirstOrDefault(p => p.Name == product.Name);

                Assert.Equal(product.Name, productFromResult!.Name);
                Assert.Equal(product.Brand, productFromResult!.Brand);
                Assert.Equal(product.Volume, productFromResult!.Volume);

                foreach (var price in product.Prices)
                {
                    var priceFromResult = productFromResult.Prices.FirstOrDefault(s => s.StoreName == price.Store.Name);
                    Assert.Equal(price.Value, priceFromResult!.Value);


                    foreach (var suggestion in price.Suggestions)
                    {
                        var suggestionFromResult = priceFromResult.Suggestions.FirstOrDefault(s => s.Autor == suggestion.AutorName);
                        Assert.Equal(suggestion.Value, suggestionFromResult!.Value);
                        Assert.Equal(suggestion.AutorName, suggestionFromResult!.Autor);
                    }
                }
            }

            productRepositoryMock.Verify(p => p.GetProductsAsync(query), Times.Once);
        }

        [Fact]
        public async void GetProductsQueryHandlerTest_WithoutPriceSuggestions_FromBase_OnSuccess()
        {
            var productList = new List<Product>()
            {
                ProductMocks.GetValidProductWithoutPriceSuggestion(),
                ProductMocks.GetValidProductWithoutPriceSuggestion()
            };

            DifferentiatingProducts(productList);

            var query = "";

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock
                .Setup(p => p.GetProductsAsync(query))
                .ReturnsAsync(productList);

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock
                .Setup(cs => cs.GetAsync<List<Product>>(It.IsAny<string>()))
                .ReturnsAsync((List<Product>)default!);

            var configurationMock = new Mock<IConfiguration>();

            var getProductsQuery = new GetProductsQuery(query);
            var sut = new GetProductsQueryHandler(productRepositoryMock.Object, cacheServiceMock.Object, configurationMock.Object);
            var result = await sut.Handle(getProductsQuery, new CancellationToken());

            Assert.NotNull(result);
            foreach (var product in productList)
            {
                var productFromResult = result.FirstOrDefault(p => p.Name == product.Name);

                Assert.Equal(product.Name, productFromResult!.Name);
                Assert.Equal(product.Brand, productFromResult!.Brand);
                Assert.Equal(product.Volume, productFromResult!.Volume);

                foreach (var price in product.Prices)
                {
                    var priceFromResult = productFromResult.Prices.FirstOrDefault(s => s.StoreName == price.Store.Name);
                    Assert.Equal(price.Value, priceFromResult!.Value);
                    Assert.Empty(priceFromResult.Suggestions);
                }
            }

            productRepositoryMock.Verify(p => p.GetProductsAsync(query), Times.Once);
        }


        [Fact]
        public async void GetProductsQueryHandlerTest_WithoutPrice_FromBase_OnSuccess()
        {
            var productList = new List<Product>()
            {
                ProductMocks.GetValidProductWithoutPrice(),
                ProductMocks.GetValidProductWithoutPrice()
            };

            DifferentiatingProducts(productList);

            var query = "";

            var productRepositoryMock = new Mock<IProductRepository>();
            productRepositoryMock
                .Setup(p => p.GetProductsAsync(query))
                .ReturnsAsync(productList);

            var cacheServiceMock = new Mock<ICacheService>();
            cacheServiceMock
                .Setup(cs => cs.GetAsync<List<Product>>(It.IsAny<string>()))
                .ReturnsAsync((List<Product>)null!);
            var configurationMock = new Mock<IConfiguration>();

            var getProductsQuery = new GetProductsQuery(query);
            var sut = new GetProductsQueryHandler(productRepositoryMock.Object, cacheServiceMock.Object, configurationMock.Object);
            var result = await sut.Handle(getProductsQuery, new CancellationToken());

            Assert.NotNull(result);
            foreach (var product in productList)
            {
                var productFromResult = result.FirstOrDefault(p => p.Name == product.Name);

                Assert.Equal(product.Name, productFromResult!.Name);
                Assert.Equal(product.Brand, productFromResult!.Brand);
                Assert.Equal(product.Volume, productFromResult!.Volume);
                Assert.Empty(productFromResult.Prices);
            }

            productRepositoryMock.Verify(p => p.GetProductsAsync(query), Times.Once);
        }
    }
}