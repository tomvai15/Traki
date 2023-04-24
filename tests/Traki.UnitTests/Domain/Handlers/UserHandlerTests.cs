using Microsoft.Extensions.Options;
using Moq;
using Traki.Domain.Constants;
using Traki.Domain.Cryptography;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Email;

namespace Traki.UnitTests.Domain.Handlers
{
    public class UserHandlerTests
    {
        private readonly IUserHandler _userHandler;
        private readonly Mock<IUsersRepository> _usersRepositoryMock;
        private readonly Mock<IHasherAdapter> _hasherAdapterMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly IOptions<WebSettings> _webSettings;

        public UserHandlerTests()
        {
            _usersRepositoryMock = new Mock<IUsersRepository>();
            _hasherAdapterMock = new Mock<IHasherAdapter>();
            _emailServiceMock = new Mock<IEmailService>();
            _webSettings = Options.Create(new WebSettings { Url = "https://example.com" });

            _userHandler = new UserHandler(_usersRepositoryMock.Object, _hasherAdapterMock.Object, _emailServiceMock.Object, _webSettings);
        }

        [Fact]
        public async Task CreateUser_ShouldAddUserToRepository()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };

            _hasherAdapterMock
                .Setup(x => x.HashText(It.IsAny<string>()))
                .Returns("hashed_password");

            // Act
            await _userHandler.CreateUser(user);

            // Assert
            _usersRepositoryMock.Verify(x => x.AddNewUser(It.Is<User>(u => u == user)), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ShouldSendActivationEmail()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var hashedPassword = "hashed_password";

            _hasherAdapterMock
                .Setup(x => x.HashText(It.IsAny<string>()))
                .Returns(hashedPassword);

            // Act
            await _userHandler.CreateUser(user);

            // Assert
            _emailServiceMock.Verify(
                x => x.SendEmail(user.Email, "Activate account", It.IsAny<string>()),
                Times.Once);
        }
    }
}
