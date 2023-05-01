using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
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
    public class DefectsRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public DefectsRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetDefect_ReturnsDefect()
        {
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectsRepository(context, _mapper);
            var defectEntity = await CreateDefect(context);

            var defect = await repository.GetDefect(defectEntity.Id);

            defectEntity.Should().BeEquivalentTo(defect, options => options.Excluding(x => x.Author));
        }

        [Fact]
        public async Task CreateDefect_CreatesDefect()
        {
            var defect = new Defect
            {
                Title = Any<string>(),
                Description = Any<string>(),
                Status = DefectStatus.NotDefect,
                ImageName= Any<string>(),
                DrawingId = 1,
                AuthorId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectsRepository(context, _mapper);

            var createdDefect = await repository.CreateDefect(defect);

            createdDefect.Should().BeEquivalentTo(defect, options => options.Excluding(x => x.Id)
                        .Excluding(x => x.DefectComments)
                        .Excluding(x => x.StatusChanges));
        }

        [Fact]
        public async Task UpdateDefect_UpdatesDefect()
        {
            // Arrange
            var defectEntity = new DefectEntity
            {
                Title = Any<string>(),
                Description = Any<string>(),
                Status = DefectStatus.NotDefect,
                ImageName = Any<string>(),
                DrawingId = 1,
                AuthorId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectsRepository(context, _mapper);

            context.Defects.Add(defectEntity);
            var createdEntity = context.SaveChangesAsync();

            var defect = new Defect
            {
                Id = defectEntity.Id,
                Status = DefectStatus.NotDefect
            };

            // Act
            var updatedDefect = await repository.UpdateDefect(defect);

            // Assert
            updatedDefect.Status.Should().Be(defect.Status);
        }

        [Fact]
        public async Task GetDefectsByQuery_ReturnsDefects()
        {
            // Arrange
            Func<Defect, bool> query = (x) => x.Status == DefectStatus.NotDefect;

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DefectsRepository(context, _mapper);

            var expectedDefects = new DefectEntity[]
            {
                await CreateDefect(context),
                await CreateDefect(context)
            };

            // Act
            var defects = await repository.GetDefectsByQuery(query);

            // Assert
            expectedDefects.Should().BeEquivalentTo(defects, options => options.Excluding(x => x.Author)
                    .Excluding(x => x.Drawing)
                    .Excluding(x => x.DefectComments)
                    .Excluding(x => x.StatusChanges));
        }

        private async  Task<DefectEntity> CreateDefect (TrakiDbContext context)
        {
            var defectEntity = new DefectEntity
            {
                Title = Any<string>(),
                Description = Any<string>(),
                Status = DefectStatus.NotDefect,
                ImageName = Any<string>(),
                DrawingId = 1,
                AuthorId = 1,
            };

            context.Defects.Add(defectEntity);
            context.SaveChangesAsync();

            return defectEntity;
        }
    }
}
