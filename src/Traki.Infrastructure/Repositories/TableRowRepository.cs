using AutoMapper;
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

        public async Task CreateTableRow(TableRow tableRow)
        {
            try
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
            catch (Exception ex) {
                return;
            }
        }
    }
}
