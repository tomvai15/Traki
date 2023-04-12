using Traki.Domain.Models.Section;

namespace Traki.Domain.Repositories
{
    public interface ITableRowRepository
    {
        Task CreateTableRow(TableRow tableRow);
    }
}
