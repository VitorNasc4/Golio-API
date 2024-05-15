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
    public class GetPriceByIdQueryHandlerTest
    {
        [Fact]
        public async void GetPriceByIdQueryHandlerTest_WithSuggestions_OnSuccess()
        {
            var price = PriceMocks.GetValidPriceWithSuggestions();

            var priceRepositoryMock = new Mock<IPriceRepository>();
            priceRepositoryMock
                .Setup(p => p.GetPriceByIdAsync(price.Id))
                .ReturnsAsync(price);

            var getPriceByIdQuery = new GetPriceByIdQuery(price.Id);
            var sut = new GetPriceByIdQueryHandler(priceRepositoryMock.Object);
            var result = await sut.Handle(getPriceByIdQuery, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(price.Value, result.Value);
            Assert.Equal(price.Store.Name, result.StoreName);

            foreach (var suggestion in price.Suggestions)
            {
                var suggestionResult = result.Suggestions.FirstOrDefault(s => s.Autor == suggestion.AutorName);
                Assert.Equal(suggestion.Value, suggestionResult!.Value);
                Assert.Equal(suggestion.AutorName, suggestionResult!.Autor);
            }


            priceRepositoryMock.Verify(p => p.GetPriceByIdAsync(price.Id), Times.Once);
        }

        [Fact]
        public async void GetPriceByIdQueryHandlerTest_WithoutSuggestions_OnSuccess()
        {
            var price = PriceMocks.GetValidPriceWithoutSuggestion();

            var priceRepositoryMock = new Mock<IPriceRepository>();
            priceRepositoryMock
                .Setup(p => p.GetPriceByIdAsync(price.Id))
                .ReturnsAsync(price);

            var getPriceByIdQuery = new GetPriceByIdQuery(price.Id);
            var sut = new GetPriceByIdQueryHandler(priceRepositoryMock.Object);
            var result = await sut.Handle(getPriceByIdQuery, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(price.Value, result.Value);
            Assert.Equal(price.Store.Name, result.StoreName);
            Assert.Empty(result.Suggestions);

            priceRepositoryMock.Verify(p => p.GetPriceByIdAsync(price.Id), Times.Once);
        }
    }
}