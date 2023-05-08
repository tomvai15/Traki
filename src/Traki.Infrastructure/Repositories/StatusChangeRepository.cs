using AutoMapper;
using Traki.Domain.Models.Drawing;
using Traki.Domain.Repositories;
using Traki.Infrastructure.Data;
using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Infrastructure.Repositories
{
    public class StatusChangeRepository : IStatusChangeRepository
    {
        private readonly TrakiDbContext _context;
        private readonly IMapper _mapper;

        public StatusChangeRepository(TrakiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreateStatusChange(StatusChange statusChange)
        {
            var statusChangeEntity = _mapper.Map<StatusChangeEntity>(statusChange);

            statusChangeEntity.Date = DateTime.Now.ToString("s");

            _context.StatusChanges.Add(statusChangeEntity);
            await _context.SaveChangesAsync();
        }
    }
}
