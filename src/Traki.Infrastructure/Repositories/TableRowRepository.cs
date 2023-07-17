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
        public Task<TableRow> CreateTableRow(TableRow tableRow)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTableRow(int tableRowId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TableRow>> GetTableRows(int tableId)
        {
            throw new NotImplementedException();
        }
    }
}
