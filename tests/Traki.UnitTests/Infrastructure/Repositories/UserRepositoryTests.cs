using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using Traki.Domain.Constants;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;
using Product = Traki.Domain.Models.Product;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class UserRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public UserRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetUsers_ReturnsUsers()
        {
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new UserRepository(context, _mapper);
            var expectedUsers = await context.Users.ToListAsync();

            var users = await repository.GetUsers();

            expectedUsers.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetUserById_ReturnsUser()
        {
            int userId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new UserRepository(context, _mapper);
            var expectedUser = await context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

            var user = await repository.GetUserById(userId);

            expectedUser.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task GetUserByEmail_ReturnsUser()
        {
            string email = "vainoristomas@gmail.com";
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new UserRepository(context, _mapper);
            var expectedUser = await context.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            var user = await repository.GetUserByEmail(email);

            expectedUser.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task AddNewUser_CreatesUser()
        {
            var user = Any<Traki.Domain.Models.User>();
            user.Id = 0;
            
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new UserRepository(context, _mapper);

            var cratedUser = await repository.AddNewUser(user);

            cratedUser.Should().BeEquivalentTo(user, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task GetUsersByQuery_ReturnsSelectedUsers()
        {
            // Arrange
            Func<Traki.Domain.Models.User, bool> query = (x) => x.Status == UserStatus.Active;

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new UserRepository(context, _mapper);

            var expectedUsers = await context.Users.Where(x => x.Status == "Active").ToListAsync();

            // Act
            var users = await repository.GetUsersByQuery(query);

            // Assert
            expectedUsers.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task UpdateUser_UpdatesUser()
        {
            var user = new Traki.Domain.Models.User
            {
                Id = 1,
                Status = Any<string>(),
                EncryptedRefreshToken = Any<string>(),
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new UserRepository(context, _mapper);

            var updatedUser = await repository.UpdateUser(user);

            updatedUser.Status.Should().Be(user.Status);
            updatedUser.EncryptedRefreshToken.Should().Be(user.EncryptedRefreshToken);
        }
    }
}
