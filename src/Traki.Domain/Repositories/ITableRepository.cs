

using Traki.Domain.Models;

namespace Traki.Domain.Repositories
{
    public interface ITableRepository
    {
        Task<Table> GetSectionTable(int sectionId);
        Task DeleteTable(int tableId);
        Task<Table> CreateTable(Table table);
    }
}
