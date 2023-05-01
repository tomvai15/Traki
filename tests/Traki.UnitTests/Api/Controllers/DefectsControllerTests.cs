using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Contracts.User;
using Traki.Api.Controllers;
using Traki.Domain.Constants;
using Traki.Domain.Handlers;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Helpers;
using User = Traki.Domain.Models.User;

namespace Traki.UnitTests.Api.Controllers
{
    public class DefectsControllerTests
    {
        private readonly Mock<IDefectHandler> _defectHandler = new Mock<IDefectHandler>();
        private readonly Mock<IDefectsRepository> _defectsRepository = new Mock<IDefectsRepository>();
        private readonly Mock<IClaimsProvider> _claimsProvider = new Mock<IClaimsProvider>();
        private readonly IMapper _mapper;
        private readonly DefectsController _controller;

        public DefectsControllerTests()
        {
            _mapper = CreateMapper();
            _controller = new DefectsController(_defectHandler.Object, _defectsRepository.Object, _claimsProvider.Object, _mapper);
        }


        [Fact]
        public async Task CreateDefect()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var items = _mapper.Map<Defect>(ExampleData.Defects.First());

            var request = new CreateDefectRequest
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            _defectHandler.Setup(x => x.CreateDefect(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Defect>())).ReturnsAsync(items);

            // Act
            var result = await _controller.CreateDefect(drawingId, request);

            // Assert
            var data = result.ShouldBeOfType<GetDefectResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task UpdateDefect()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var defectId = 1;
            var items = _mapper.Map<Defect>(ExampleData.Defects.First());

            var request = new CreateDefectRequest
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            _defectHandler.Setup(x => x.CreateDefectStatusChange(It.IsAny<int>(), It.IsAny<Defect>())).ReturnsAsync(items);

            // Act
            var result = await _controller.UpdateDefect(drawingId, defectId, request);

            // Assert
            var data = result.ShouldBeOfType<GetDefectResponse>();
            response.Should().BeEquivalentTo(data);
        }


        [Fact]
        public async Task GetDefect()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var defectId = 1;
            var items = _mapper.Map<Defect>(ExampleData.Defects.First());

            var response = new GetDefectResponse
            {
                Defect = _mapper.Map<DefectDto>(items)
            };

            _defectsRepository.Setup(x => x.GetDefect(It.IsAny<int>())).ReturnsAsync(items);

            // Act
            var result = await _controller.GetDefect(drawingId, defectId);

            // Assert
            var data = result.ShouldBeOfType<GetDefectResponse>();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task AddDefectComment()
        {
            // Arrange
            var userId = 1;
            var drawingId = 1;
            var defectId = 1;
            var items = new DefectComment();

            var request = new CreateDefectCommentRequest
            {
                DefectComment = _mapper.Map<DefectCommentDto>(items)
            };

            _defectHandler.Setup(x => x.CreateDefectComment(It.IsAny<int>(), It.IsAny<DefectComment>()));

            // Act
            var result = await _controller.AddDefectComment(drawingId, defectId, request);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        /*
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
        }*/
    }
}
