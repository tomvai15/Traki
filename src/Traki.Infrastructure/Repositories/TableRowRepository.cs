using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Repositories
{
    public class TableRowRepository : ITableRowRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public TableRowRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteTableRow(int tableRowId)
        {
            var tableRowEntity = _context.TableRows.Where(x => x.Id == tableRowId).First();

            if (tableRowEntity == null)
            {
                return;
            }

            _context.TableRows.Remove(tableRowEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<TableRow> CreateTableRow(TableRow tableRow)
        {
            var tableRowEntity = _mapper.Map<TableRowEntity>(tableRow);

            tableRowEntity.Id = 0;
            foreach (var column in tableRowEntity.RowColumns)
            {
                column.Id = 0;
                column.TableRowId = tableRowEntity.Id;
            }

            _context.TableRows.Add(tableRowEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TableRow>(tableRow);
        }

        public async Task<IEnumerable<TableRow>> GetTableRows(int tableId)
        {
            var tableRowEntities = await _context.TableRows.Where(x => x.TableId == tableId).ToListAsync();
            return _mapper.Map<IEnumerable<TableRow>>(tableRowEntities);
        }
    }
}
