using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Traki.Api.Contracts.Drawing.Defect;
using Traki.Api.Controllers;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Providers;
using Traki.Domain.Repositories;
using Traki.UnitTests.Helpers;

namespace Traki.UnitTests.Api.Controllers
{
    public class NotificationsControllerTests
    {
        private readonly Mock<IDefectNotificationRepository> _defectNotificationRepository;
        private readonly Mock<IClaimsProvider> claimsProvider;
        private readonly IMapper _mapper;
        private readonly NotificationsController _controller;

        public NotificationsControllerTests()
        {
            _defectNotificationRepository = new Mock<IDefectNotificationRepository>();
            claimsProvider = new Mock<IClaimsProvider>();
            _mapper = CreateMapper();
            _controller = new NotificationsController(_defectNotificationRepository.Object, claimsProvider.Object, _mapper);
        }

        [Fact]
        public async Task GetNotifications()
        {
            // Arrange
            var projectId = 1;
            var items = new List<DefectNotification>();
            var response = new GetDefectNotificationsResponse
            {
                DefectNotifications = _mapper.Map<IEnumerable<DefectNotificationDto>>(items)
            };

            _defectNotificationRepository.Setup(repo => repo.GetUserDefectNotifications(It.IsAny<int>()))
                .ReturnsAsync(items);

            // Act
            var result = await _controller.GetNotifications();

            // Assert
            var data = result.ShouldBeOfType();
            response.Should().BeEquivalentTo(data);
        }

        [Fact]
        public async Task DeleteNotification()
        {
            // Arrange
            var defectId = 1;
            var items = new List<DefectNotification>();
            var response = new GetDefectNotificationsResponse
            {
                DefectNotifications = _mapper.Map<IEnumerable<DefectNotificationDto>>(items)
            };

            _defectNotificationRepository.Setup(repo => repo.DeleteDefectNotification(It.IsAny<int>(), defectId));

            // Act
            var result = await _controller.DeleteNotification(defectId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}
