using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.User;
using Traki.Api.Controllers;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Domain.Repositories;
using Traki.UnitTests.Helpers;
using User = Traki.Domain.Models.User;

namespace Traki.UnitTests.Api.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserAuthHandler> _userAuthHandler = new Mock<IUserAuthHandler>();
        private readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        private readonly Mock<IUserHandler> _userHandler = new Mock<IUserHandler>();
        private readonly IMapper _mapper;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _mapper = CreateMapper();
            _controller = new UsersController(_userHandler.Object, _userAuthHandler.Object, _usersRepository.Object, _mapper);
        }

        [Fact]
        public async Task GetUserByRegisterId()
        {
            // Arrange
            var registerId = "test";
            var items = new List<User> { new User {
                    Email = "test",
                    Id = 0,
                    Name = "test",
                    Role = "test",
                    Status = "test",
                    Surname = "test",
                    UserIconBase64 = "test",
                }
            };

            var response = new GetUserResponse
            {
                User = _mapper.Map<UserDto>(items.First())
            };

            _usersRepository.Setup(x => x.GetUsersByQuery(It.IsAny<Func<User, bool>>())).ReturnsAsync(items);

            // Act
            var result = await _controller.GetUserByRegisterId(registerId);

            // Assert
            var data = result.ShouldBeOfType<GetUserResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task GetUser()
        {
            // Arrange
            var userId = 1;
            var items = new User
            {
                Email = "test",
                Id = 0,
                Name = "test",
                Role = "test",
                Status = "test",
                Surname = "test",
                UserIconBase64 = "test",
            };

            var response = new GetUserResponse
            {
                User = _mapper.Map<UserDto>(items)
            };

            _usersRepository.Setup(x => x.GetUserById(userId)).ReturnsAsync(items);

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            var data = result.ShouldBeOfType<GetUserResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task UpdateStatus()
        {
            // Arrange
            var userId = 1;
            var items = new User
            {
                Email = "test",
                Id = 0,
                Name = "test",
                Role = "test",
                Status = "test",
                Surname = "test",
                UserIconBase64 = "test",
            };

            var request = new UpdateUserStatusRequest
            {
                Status = UserStatus.Active
            };

            _usersRepository.Setup(x => x.GetUserById(userId)).ReturnsAsync(items);

            // Act
            var result = await _controller.UpdateStatus(userId, request);

            // Assert
            result.Should().BeOfType<OkResult>();
            _usersRepository.Verify(x => x.UpdateUser(items));
        }

        [Fact]
        public async Task UpdateStatus_IncorrectStatus()
        {
            // Arrange
            var userId = 1;
            var items = new User
            {
                Email = "test",
                Id = 0,
                Name = "test",
                Role = "test",
                Status = "test",
                Surname = "test",
                UserIconBase64 = "test",
            };

            var request = new UpdateUserStatusRequest
            {
                Status = UserStatus.Created
            };

            _usersRepository.Setup(x => x.GetUserById(userId)).ReturnsAsync(items);

            // Act
            var result = await _controller.UpdateStatus(userId, request);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task CreateUser()
        {
            // Arrange
            var userId = 1;
            var items = new User
            {
                Email = "test",
                Id = 0,
                Name = "test",
                Role = "test",
                Status = "test",
                Surname = "test",
                UserIconBase64 = "test",
            };

            var request = new CreateUserRequest
            {
                User = _mapper.Map<UserDto>(items)
            };

            // Act
            var result = await _controller.CreateUser(request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task GetUsers()
        {
            // Arrange
            var userId = 1;
            var items = new List<User> { new User {
                    Email = "test",
                    Id = 0,
                    Name = "test",
                    Role = "test",
                    Status = "test",
                    Surname = "test",
                    UserIconBase64 = "test",
                }
            };

            var response = new GetUsersResponse
            {
                Users = _mapper.Map<IEnumerable<UserDto>>(items)
            };

            _usersRepository.Setup(x => x.GetUsers()).ReturnsAsync(items);

            // Act
            var result = await _controller.GetUsers();

            // Assert
            var data = result.ShouldBeOfType<GetUsersResponse>();
            response.Should().BeEquivalentTo(data);
        }
    }
}
