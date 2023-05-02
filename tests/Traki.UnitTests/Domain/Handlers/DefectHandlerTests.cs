using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Exceptions;
using Traki.Domain.Handlers;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Domain.Services.Notifications;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Domain.Handlers
{
    public class DefectHandlerTests
    {
        private readonly Mock<IDefectsRepository> _defectsRepository = new Mock<IDefectsRepository>();
        private readonly Mock<INotificationService> _notificationService = new Mock<INotificationService>();
        private readonly Mock<IDrawingsRepository> _drawingsRepository = new Mock<IDrawingsRepository>();
        private readonly Mock<IProductsRepository> _productsRepository = new Mock<IProductsRepository>();
        private readonly Mock<IUsersRepository> _usersRepository = new Mock<IUsersRepository>();
        private readonly Mock<IDefectNotificationRepository> _defectNotificationRepository = new Mock<IDefectNotificationRepository>();
        private readonly Mock<IStatusChangeRepository> _statusChangeRepository = new Mock<IStatusChangeRepository>();
        private readonly Mock<IDefectCommentRepository> _defectCommentRepository = new Mock<IDefectCommentRepository>();

        private readonly DefectHandler _defectHandler;

        public DefectHandlerTests()
        {
            _defectHandler = new DefectHandler(_defectCommentRepository.Object, _statusChangeRepository.Object, _defectsRepository.Object,
                _notificationService.Object, _drawingsRepository.Object, _productsRepository.Object, 
                _usersRepository.Object, _defectNotificationRepository.Object);
        }

        [Fact] 
        public async Task CreateDefect()
        {
            int userId = 1;
            int drawingId = 1;
            var defect = MockData.Defects.First();

            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(MockData.Users.First());
            _defectsRepository.Setup(x => x.CreateDefect(It.IsAny<Defect>())).ReturnsAsync(MockData.Defects.First());

            _drawingsRepository.Setup(x => x.GetDrawing(It.IsAny<int>())).ReturnsAsync(MockData.Drawings.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());

            await _defectHandler.CreateDefect(userId, drawingId, defect);

            Assert.True(true);
        }

        [Fact]
        public async Task CreateDefectComment()
        {
            int userId = 1;
            int drawingId = 1;
            var defectComment = MockData.DefectComments.First();

            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(MockData.Users.First());
            _defectsRepository.Setup(x => x.CreateDefect(It.IsAny<Defect>())).ReturnsAsync(MockData.Defects.First());
            _defectsRepository.Setup(x => x.GetDefect(It.IsAny<int>())).ReturnsAsync(MockData.Defects.First());

            _drawingsRepository.Setup(x => x.GetDrawing(It.IsAny<int>())).ReturnsAsync(MockData.Drawings.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());

            await _defectHandler.CreateDefectComment(userId, defectComment);

            Assert.True(true);
        }

        [Fact]
        public async Task CreateDefectStatusChange_ShouldThrow()
        {
            int userId = 1;
            int drawingId = 1;
            var defect = MockData.Defects.First();

            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(MockData.Users.First());
            _defectsRepository.Setup(x => x.CreateDefect(It.IsAny<Defect>())).ReturnsAsync(MockData.Defects.First());
            _defectsRepository.Setup(x => x.GetDefect(It.IsAny<int>())).ReturnsAsync(MockData.Defects.First());

            _drawingsRepository.Setup(x => x.GetDrawing(It.IsAny<int>())).ReturnsAsync(MockData.Drawings.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());

            var func = async () => await _defectHandler.CreateDefectStatusChange(userId, defect);


            func.Should().ThrowAsync<BadOperationException>();
        }

        [Fact]
        public async Task CreateDefectStatusChange_StatusFixed_ShouldThrow()
        {
            int userId = 5;
            int drawingId = 1;
            var defect = MockData.Defects.First();
            defect.Status = DefectStatus.Fixed;

            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(MockData.Users.First());
            _defectsRepository.Setup(x => x.CreateDefect(It.IsAny<Defect>())).ReturnsAsync(MockData.Defects.First());
            _defectsRepository.Setup(x => x.GetDefect(It.IsAny<int>())).ReturnsAsync(MockData.Defects.First());

            _drawingsRepository.Setup(x => x.GetDrawing(It.IsAny<int>())).ReturnsAsync(MockData.Drawings.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());

            var func = async () => await _defectHandler.CreateDefectStatusChange(userId, defect);


            func.Should().ThrowAsync<ForbbidenOperationException>();
        }


        [Fact]
        public async Task CreateDefectStatusChange()
        {
            int userId = 1;
            int drawingId = 1;
            var defect = MockData.Defects.First();

            defect.Status = DefectStatus.Unfixable;

            _usersRepository.Setup(x => x.GetUserById(It.IsAny<int>())).ReturnsAsync(MockData.Users.First());
            _defectsRepository.Setup(x => x.CreateDefect(It.IsAny<Defect>())).ReturnsAsync(MockData.Defects.First());
            _defectsRepository.Setup(x => x.UpdateDefect(It.IsAny<Defect>())).ReturnsAsync(MockData.Defects.First());
            _defectsRepository.Setup(x => x.GetDefect(It.IsAny<int>())).ReturnsAsync(MockData.Defects.First());

            _drawingsRepository.Setup(x => x.GetDrawing(It.IsAny<int>())).ReturnsAsync(MockData.Drawings.First());
            _productsRepository.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(MockData.Products.First());

            await _defectHandler.CreateDefectStatusChange(userId, defect);

            Assert.True(true);
        }


    }
}
