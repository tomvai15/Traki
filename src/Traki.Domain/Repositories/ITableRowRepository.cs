

using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface ITableRowRepository
    {
        Task<TableRow> CreateTableRow(TableRow tableRow);
        Task DeleteTableRow(int tableRowId);
        Task<IEnumerable<TableRow>> GetTableRows(int tableId);
    }
}
