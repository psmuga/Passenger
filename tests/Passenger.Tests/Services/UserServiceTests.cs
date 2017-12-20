using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Passenger.Core.Domain;
using Passenger.Core.Repositories;
using Passenger.Infrastructure.Services;
using Xunit;

namespace Passenger.Tests.Services
{
    public class UserServiceTests
    {
        [Fact]
        public async Task register_async_should_invoke_add_async_on_repository()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var mapperMock = new Mock<IMapper>();
            var encrypterMock = new Mock<IEncrypter>();
        //Given
            var userService = new UserService(userRepositoryMock.Object,encrypterMock.Object, mapperMock.Object);
        //When
            await userService.RegisterAsync(Guid.NewGuid(),"user@email,com","user","secret","user");
        //Then
            userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>()),Times.Once);
        }
    }
}