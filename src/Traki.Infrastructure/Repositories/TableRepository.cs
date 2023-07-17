using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Repositories
{
    public class TableRepository : ITableRepository
    {
        public Task<Table> CreateTable(Table table)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTable(int tableId)
        {
            throw new NotImplementedException();
        }

        public Task<Table> GetSectionTable(int sectionId)
        {
            throw new NotImplementedException();
        }
    }
}
