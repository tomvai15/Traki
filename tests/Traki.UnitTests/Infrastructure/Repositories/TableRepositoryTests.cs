using AutoMapper;
using DocuSign.eSign.Model;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;
using Traki.Domain.Models;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Models.Section;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities;
using Traki.Infrastructure.Entities.Drawing;
using Traki.Infrastructure.Entities.Section;
using Traki.Infrastructure.Repositories;
using Traki.UnitTests.Infrastructure.Fixture;
using Product = Traki.Domain.Models.Product;

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

            expectedTable.Should().BeEquivalentTo(table, options => options.Excluding(x => x.Id)
                .Excluding(x => x.Section)
                .Excluding(x => x.TableRows));
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

            createdTable.Should().BeEquivalentTo(table, options => options.Excluding(x => x.Id)
            .Excluding(x => x.Section)
            .Excluding(x => x.TableRows));
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
