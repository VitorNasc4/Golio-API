using System.Security.Claims;
using Golio.Application.Commands.CreateProduct.SendSuggestion;
using Golio.Core.DTOs;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Core.Services;
using Golio.Test.Mocks;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class SendSuggestionCommandHandlerTest
    {
        public Mock<IHttpContextAccessor> HttpContextAccessorMockFactory(string? email = null)
        {
            // Mock do httpContextAccessor
            var httpContext = new Mock<HttpContext>();
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim("userName", email ?? "dummyEmail@email.com"),
            }, "mock");
            var userClaim = new ClaimsPrincipal(identity);
            httpContext.Setup(c => c.User).Returns(userClaim);
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContext.Object);

            return httpContextAccessorMock;
        }

        [Fact]
        public async Task InputDataIsOk_Executed()
        {
            var priceReturnedFromMock = new Price()
            {
                Id = 1,
                Value = 50
            };
            var sendSuggestionCommand = new SendSuggestionCommand
            {
                PriceId = priceReturnedFromMock.Id,
                NewPrice = priceReturnedFromMock.Value * 2,
            };
            var user = UserMocks.GetValidClientUser();

            // Mock do PriceRepository
            var priceRepositoryMock = new Mock<IPriceRepository>();
            priceRepositoryMock
                .Setup(p => p.GetPriceByIdAsync(sendSuggestionCommand.PriceId))
                .ReturnsAsync(priceReturnedFromMock);

            // Mock do UserRepository
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(p => p.GetUserByEmailAsync(user.Email))
                .ReturnsAsync(user);

            // Mock do MessageBusService
            var messageBusServiceMock = new Mock<IMessageBusService>();

            // Mock do HttpContextAccessor
            var httpContextAccessorMock = HttpContextAccessorMockFactory(user.Email);

            var sut = new SendSuggestionCommandHandler(priceRepositoryMock.Object, userRepositoryMock.Object, messageBusServiceMock.Object, httpContextAccessorMock.Object);
            var id = await sut.Handle(sendSuggestionCommand, new CancellationToken());

            priceRepositoryMock.Verify(p => p.GetPriceByIdAsync(sendSuggestionCommand.PriceId), Times.Once);
            userRepositoryMock.Verify(p => p.GetUserByEmailAsync(user.Email), Times.Once);
            messageBusServiceMock.Verify(s => s.SendMessageQueueAsync(It.Is<SuggestionDTO>(suggestion =>
                suggestion.Value == sendSuggestionCommand.NewPrice &&
                suggestion.PriceId == priceReturnedFromMock.Id &&
                suggestion.AutorName == user.FullName
                )), Times.Once);
        }
    }
}