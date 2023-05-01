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
    public class TableRowRepositoryTests
    {
        private readonly TrakiDbFixture _trakiDbFixture;
        private readonly IMapper _mapper;

        public TableRowRepositoryTests(TrakiDbFixture trakiDbFixture)
        {
            _mapper = CreateMapper();
            _trakiDbFixture = trakiDbFixture;
        }

        [Fact]
        public async Task GetTableRows_ReturnsTableRows()
        {
            int tableId = 1;
            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new TableRowRepository(context, _mapper);
            var expectedTableRows = await context.TableRows.Where(x => x.TableId == tableId).ToListAsync();

            var tableRows = await repository.GetTableRows(tableId);

            expectedTableRows.Should().BeEquivalentTo(tableRows, options => options.Excluding(x => x.RowColumns));
        }

        [Fact]
        public async Task CreateTableRow_CreatesTableRow()
        {
            var tableRow = new TableRow
            {
                TableId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new TableRowRepository(context, _mapper);

            var createdTableRow = await repository.CreateTableRow(tableRow);

            createdTableRow.Should().BeEquivalentTo(tableRow, options => options.Excluding(x => x.Id)
            .Excluding(x => x.Table));
        }     

        [Fact]
        public async Task DeleteTableRow_DeletesTableRow()
        {
            // Arrange
            var tableRow = new TableRowEntity
            {
                TableId = 1,
            };

            using var context = new TrakiDbContext(_trakiDbFixture.Options);
            var repository = new TableRowRepository(context, _mapper);

            context.TableRows.Add(tableRow);
            var createdEntity = context.SaveChangesAsync();

            // Act
            await repository.DeleteTableRow(tableRow.Id);

            // Assert
            var foundEntity = await context.TableRows.FirstOrDefaultAsync(x => x.Id == tableRow.Id);
            foundEntity.Should().BeNull();
        }
    }
}
