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

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class DrawingsRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public DrawingsRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetDrawings_ReturnsProductsDrawings()
        {
            int productId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DrawingsRepository(context, _mapper);
            var expectedDrawings = await context.Drawings.Where(x => x.ProductId == productId)
                            .Include(x => x.Defects).ThenInclude(x => x.Author).ToListAsync();

            var result = await repository.GetDrawings(productId);

            result.Should().BeEquivalentTo(expectedDrawings, options => options.Excluding(x => x.Defects)
                .Excluding(x => x.Product));
        }

        [Fact]
        public async Task GetDrawing_ReturnsDrawing()
        {
            int drawingId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DrawingsRepository(context, _mapper);
            var expectedDrawing = await context.Drawings.Where(x => x.Id == drawingId).FirstOrDefaultAsync();

            var drawing = await repository.GetDrawing(drawingId);

            drawing.Should().BeEquivalentTo(expectedDrawing, options => options.Excluding(x => x.Defects)
                .Excluding(x => x.Product));
        }

        [Fact]
        public async Task CreateDrawing_CreatesDrawing()
        {
            var drawing = new Drawing
            {
                Title = Any<string>(),
                ImageName = Any<string>(),
                ProductId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DrawingsRepository(context, _mapper);

            var cratedDrawing = await repository.CreateDrawing(drawing);

            cratedDrawing.Title.Should().Be(drawing.Title);
            cratedDrawing.ImageName.Should().Be(drawing.ImageName);
            cratedDrawing.ProductId.Should().Be(drawing.ProductId);
        }

        [Fact]
        public async Task DeleteDrawing_DeletesDrawing()
        {
            // Arrange
            var drawing = new DrawingEntity
            {
                Title = Any<string>(),
                ImageName = Any<string>(),
                ProductId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new DrawingsRepository(context, _mapper);

            context.Drawings.Add(drawing);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteDrawing(drawing.Id);

            // Assert
            var foundEntity = await context.Projects.FirstOrDefaultAsync(x => x.Id == drawing.Id);
            foundEntity.Should().BeNull();
        }
    }
}
