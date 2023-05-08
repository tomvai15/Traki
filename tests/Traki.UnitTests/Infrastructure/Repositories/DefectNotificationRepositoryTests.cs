using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class DefectNotificationRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public DefectNotificationRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetUserDefectNotifications_ReturnsDefectNotifications()
        {
            // Arrange
            var notifications = new DefectNotificationEntity[] {
                new DefectNotificationEntity
                {
                    Title = Any<string>(),
                    Body = Any<string>(),
                    Data = Any<string>(),
                    CreationDate = Any<string>(),
                    UserId = 1,
                    DefectId = 1
                },
                new DefectNotificationEntity
                {
                    Title = Any<string>(),
                    Body = Any<string>(),
                    Data = Any<string>(),
                    CreationDate = Any<string>(),
                    UserId = 1,
                    DefectId = 1
                }};

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectNotificationRepository(context, _mapper);

            context.DefectNotifications.AddRange(notifications);
            var createdEntity = context.SaveChangesAsync();

            // Act
            var result = await repository.GetUserDefectNotifications(1);

            // Assert
            result.Should().BeEquivalentTo(notifications, options => options.Excluding(x => x.User)
                .Excluding(x => x.Defect));
        }

        [Fact]
        public async Task CreateDefectNotification_CreatesDefectNotification()
        {
            var notification = new DefectNotification
            {
                Title = "test",
                Body = "test",
                Data = "test",
                UserId = 1,
                DefectId = 1
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectNotificationRepository(context, _mapper);

            var createdNotification = await repository.CreateDefectNotification(notification);

            createdNotification.Title.Should().Be(notification.Title);
            createdNotification.Body.Should().Be(notification.Body);
            createdNotification.Data.Should().Be(notification.Data);
            createdNotification.UserId.Should().Be(notification.UserId);
            createdNotification.DefectId.Should().Be(notification.DefectId);
        }

        [Fact]
        public async Task DeleteDefectNotification_DeletesDefectNotification()
        {
            // Arrange
            int userId = 2;
            int defectId = 1;
            var notification = new DefectNotificationEntity
            {
                Title = "test",
                Body = "test",
                Data = "test",
                CreationDate = "Test",
                UserId = userId,
                DefectId = defectId
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectNotificationRepository(context, _mapper);

            context.DefectNotifications.Add(notification);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteDefectNotification(userId, defectId);

            // Assert
            var foundEntity = await context.DefectNotifications.FirstOrDefaultAsync(x => x.Id == notification.Id);
            foundEntity.Should().BeNull();
        }
    }
}
