using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golio.Application.Commands.UserCommands.CreateUser;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Core.Services;
using Golio.Test.Mocks;
using Moq;
using Xunit;

namespace Golio.Test.Application.Commands
{
    public class CreateUserCommandHandlerTest
    {
        [Fact]
        public async Task InputDataIsOk_Executed_ReturnUserId()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var authServiceMock = new Mock<IAuthService>();

            var createUserCommand = new CreateUserCommand
            {
                Email = "email@email.com",
                FullName = "Teste",
                Password = "Teste",
            };

            var sut = new CreateUserCommandHandler(userRepositoryMock.Object, authServiceMock.Object);
            var id = await sut.Handle(createUserCommand, new CancellationToken());

            Assert.True(id >= 0);
            userRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<User>()), Times.Once);
            authServiceMock.Verify(a => a.ComputeSha256Hash(createUserCommand.Password), Times.Once);
        }
    }
}