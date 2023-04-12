using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Section;
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

        public async Task DeleteTableRow(TableRow tableRow)
        {
            var tableRowEntity = _context.TableRows.Where(x => x.Id == tableRow.Id).First();

            if (tableRowEntity == null)
            {
                return;
            }

            _context.TableRows.Remove(tableRowEntity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateTableRow(TableRow tableRow)
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
        }

        public async Task<IEnumerable<TableRow>> GetTableRows(int tableId)
        {
            var tableRowEntities = await _context.TableRows.Where(x => x.TableId == tableId).ToListAsync();
            return _mapper.Map<IEnumerable<TableRow>>(tableRowEntities);
        }
    }
}
