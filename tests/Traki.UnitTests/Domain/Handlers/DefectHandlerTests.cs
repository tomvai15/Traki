using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Notifications;

namespace Traki.UnitTests.Domain.Handlers
{
    public class DefectHandlerTests
    {
        private readonly Mock<IDefectsRepository> _defectsRepositoryMock = new Mock<IDefectsRepository>();
        private readonly Mock<INotificationService> _notificationServiceMock = new Mock<INotificationService>();
        private readonly Mock<IDrawingsRepository> _drawingsRepositoryMock = new Mock<IDrawingsRepository>();
        private readonly Mock<IProductsRepository> _productsRepositoryMock = new Mock<IProductsRepository>();
        private readonly Mock<IUsersRepository> _usersRepositoryMock = new Mock<IUsersRepository>();
        private readonly Mock<IDefectNotificationRepository> _defectNotificationRepositoryMock = new Mock<IDefectNotificationRepository>();

        private readonly DefectHandler _defectHandler;

        public DefectHandlerTests()
        {
            _defectHandler = new DefectHandler(
                _defectsRepositoryMock.Object,
                _notificationServiceMock.Object,
                _drawingsRepositoryMock.Object,
                _productsRepositoryMock.Object,
                _usersRepositoryMock.Object,
                _defectNotificationRepositoryMock.Object);
        }

        [Fact]
        public async Task CreateDefect_ShouldCreateDefectAndSendNotification_WhenDeviceTokenIsNotEmpty()
        {
            // Arrange
            int userId = 1;
            int drawingId = 2;
            Defect defect = new Defect { Description = "Test Defect" };

            var drawing = new Drawing { Id = drawingId, ProductId = 3 };
            var product = new Product { Id = drawing.ProductId, UserId = 4 };
            var user = new User { Id = product.UserId, DeviceToken = "TestDeviceToken" };

            _drawingsRepositoryMock.Setup(x => x.GetDrawing(drawingId)).ReturnsAsync(drawing);
            _productsRepositoryMock.Setup(x => x.GetProduct(drawing.ProductId)).ReturnsAsync(product);
            _usersRepositoryMock.Setup(x => x.GetUserById(product.UserId)).ReturnsAsync(user);

            Defect savedDefect = null;
            _defectsRepositoryMock.Setup(x => x.CreateDefect(It.IsAny<Defect>()))
                .Callback<Defect>(d => savedDefect = d)
                .ReturnsAsync(() => savedDefect);

            DefectNotification savedDefectNotification = null;

            _defectNotificationRepositoryMock.Setup(x => x.CreateDefectNotification(It.IsAny<DefectNotification>()))
                .Callback<DefectNotification>(dn => savedDefectNotification = dn);

            // Act
            var result = await _defectHandler.CreateDefect(userId, drawingId, defect);

            // Assert
            _defectsRepositoryMock.Verify(x => x.CreateDefect(It.IsAny<Defect>()), Times.Once);
            _defectNotificationRepositoryMock.Verify(x => x.CreateDefectNotification(It.IsAny<DefectNotification>()), Times.Once);
            _notificationServiceMock.Verify(x => x.SendNotification(user.DeviceToken, It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            savedDefect.UserId.Should().Be(userId);
            savedDefect.DrawingId.Should().Be(drawingId);
            savedDefect.Status.Should().Be(DefectStatus.NotFixed);
            savedDefect.Description.Should().Be("Test Defect");
            result.Should().Be(defect);
        }
    }
}
