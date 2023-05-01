using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Traki.Domain.Models.Section;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Section;

namespace Traki.Infrastructure.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public TableRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Table> CreateTable(Table table)
        {
            var tableEntity = _mapper.Map<TableEntity>(table);
            _context.Tables.Add(tableEntity);
            await _context.SaveChangesAsync();

            return _mapper.Map<Table>(tableEntity);
        }

        public async Task DeleteTable(int tableId)
        {
            var table = await _context.Tables.Where(x=> x.Id == tableId).FirstOrDefaultAsync();

            if (table == null){return;}

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
        }

        public async Task<Table> GetSectionTable(int sectionId)
        {
            var tableEntity = await _context.Tables.Where(x => x.SectionId == sectionId)
                .Include(x=> x.TableRows).ThenInclude(x=> x.RowColumns)
                .FirstOrDefaultAsync();
            return _mapper.Map<Table>(tableEntity);
        }
    }
}
