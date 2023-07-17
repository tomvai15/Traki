using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;

namespace Traki.UnitTests.Infrastructure.Repositories
{
    [Collection("Sequential")]
    public class TableRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public TableRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetSectionTable_ReturnsTable()
        {
            int sectionId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new TableRepository(context, _mapper);
            var expectedTable = await context.Tables.Where(x => x.SectionId == sectionId).FirstOrDefaultAsync();

            var table = await repository.GetSectionTable(sectionId);

            expectedTable.Should().BeEquivalentTo(table, options => options.Excluding(x => x.Id));
        }

        [Fact]
        public async Task CreateTable_CreatesTable()
        {
            var table = new Table
            {
                SectionId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new TableRepository(context, _mapper);

            var createdTable = await repository.CreateTable(table);

            createdTable.Should().BeEquivalentTo(table, options => options.Excluding(x => x.Id));
        }     

        [Fact]
        public async Task DeleteTable_DeletesTable()
        {
            // Arrange
            var table = new TableEntity
            {
                SectionId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new TableRepository(context, _mapper);

            context.Tables.Add(table);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteTable(table.Id);

            // Assert
            var foundEntity = await context.Tables.FirstOrDefaultAsync(x => x.Id == table.Id);
            foundEntity.Should().BeNull();
        }
    }
}
